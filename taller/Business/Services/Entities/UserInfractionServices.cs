using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.IBusinessImplements.parameters;
using Business.Interfaces.PDF;
using Business.Mensajeria.Email.implements;
using Business.Mensajeria.Email.@interface;
using Business.Repository;
using Business.Services.Notificacion;
using Business.Strategy.StrategyGet.Implement;
using Business.Validaciones.Entities.UserInfraction;
using Data.Interfaces.IDataImplement.Entities;   // <- IUserInfractionRepository
using Data.Interfaces.IDataImplement.parameters;
using Data.Interfaces.IDataImplement.Security;   // <- IUserRepository
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.AnexarMulta;           // <- DTO especial para anexar multas con persona
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Default.Notificacion;
using Entity.DTOs.Select.Entities;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using SendGrid.Helpers.Errors.Model;
using Utilities.Exceptions;
using static Entity.Domain.Enums.Notification.NotificationEnums;

public class UserInfractionServices
    : BusinessBasic<UserInfractionDto, UserInfractionSelectDto, UserInfraction>, IUserInfractionServices
{
    private readonly ILogger<UserInfractionServices> _logger;
    private readonly IUserInfractionRepository _repo;
    private readonly IUserRepository _users;
    private readonly IInfractionRepository _types;
    private readonly IUserNotificationRepository _notifs;
    private readonly EmailBackgroundQueue _emailQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IPdfGeneratorService _pdfservices;
    private readonly ReminderEmailAppService _reminderEmailAppService;
    private readonly IServiceEmail _emailService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly INotificationSettingRepository _notificationSettingService;
    private readonly EmailOrchestrator _emailOrchestrator;
    private readonly EmailScheduler _scheduler;

    public UserInfractionServices(
        IUserInfractionRepository repo,
        IUserRepository users,
        IInfractionRepository types,
        IUserNotificationRepository notifs,
        IMapper mapper,
        ILogger<UserInfractionServices> logger,
        EmailBackgroundQueue emailQueue,
        IServiceScopeFactory scopeFactory,
        ApplicationDbContext db,
        IPdfGeneratorService pdfService,
        ReminderEmailAppService reminderEmailAppService,
        IServiceEmail emailService,
        INotificationSettingRepository notificationSettingService,
        EmailOrchestrator emailOrchestrator,
        EmailScheduler scheduler
    ) : base(repo, mapper, db)
    {
        _repo = repo;
        _users = users;
        _types = types;
        _notifs = notifs;
        _mapper = mapper;
        _logger = logger;
        _emailQueue = emailQueue;
        _scopeFactory = scopeFactory;
        _pdfservices = pdfService;
        _reminderEmailAppService = reminderEmailAppService;
        _emailService = emailService;
        _context = db;
        _notificationSettingService = notificationSettingService;
        _emailOrchestrator = emailOrchestrator;
        _scheduler = scheduler;
    }

    // -------- Helpers FK --------
    private async Task EnsureFkAsync(UserInfractionDto dto)
    {
        // Validar FK: userId
        if (await _users.GetByIdAsync(dto.userId) is null)
            throw new BusinessException($"El usuario con ID {dto.userId} no existe.");

        // Validar FK: typeInfractionId
        if (await _types.GetByIdAsync(dto.typeInfractionId) is null)
            throw new BusinessException($"El tipo de infracción con ID {dto.typeInfractionId} no existe.");

        // Validar FK: UserNotificationId
        if (await _notifs.GetByIdAsync(dto.UserNotificationId) is null)
            throw new BusinessException($"La notificación de usuario con ID {dto.UserNotificationId} no existe.");
    }

    // 🔎 Obtener infracción por ID
    public override async Task<UserInfractionSelectDto?> GetByIdAsync(int id)
    {
        BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

        var entity = await _repo.GetByIdAsync(id);
        if (entity is null)
            throw new BusinessException($"La infracción de usuario con ID {id} no existe.");

        return _mapper.Map<UserInfractionSelectDto>(entity);
    }

    // ✏️ Actualizar infracción existente
    public override async Task<bool> UpdateAsync(UserInfractionDto dto)
    {
        BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");
        BusinessValidationHelper.ThrowIfZeroOrLess(dto.id, "El ID debe ser mayor que cero.");

        if (!await ExistsAsync(dto.id))
            throw new BusinessException($"La infracción de usuario con ID {dto.id} no existe.");

        var existing = await _repo.GetByIdAsync(dto.id)
         ?? throw new BusinessException($"La infracción no existe.");

        // 🔹 PRESERVAR el valor histórico del SMLDV
        dto.smldvValueAtCreation = existing.smldvValueAtCreation
            ?? throw new BusinessException("El valor histórico del SMLDV no existe.");

        // 🔹 Recalcular amountToPay con el valor histórico
        var typeInfraction = await _types.GetByIdAsync(dto.typeInfractionId)
            ?? throw new BusinessException("Tipo de infracción inválido.");

        dto.amountToPay = typeInfraction.numer_smldv * dto.smldvValueAtCreation;

        return await base.UpdateAsync(dto);
    }

    // ❌ Eliminar infracción
    public override async Task<bool> DeleteAsync(int id)
    {
        BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

        if (!await ExistsAsync(id))
            throw new BusinessException($"No se puede eliminar. La infracción de usuario con ID {id} no existe.");

        return await base.DeleteAsync(id);
    }

    // 🔄 Restaurar lógicamente una infracción eliminada
    public override async Task<bool> RestoreLogical(int id)
    {
        BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

        if (!await ExistsAsync(id))
            throw new BusinessException($"No se puede restaurar. La infracción de usuario con ID {id} no existe.");

        return await base.RestoreLogical(id);
    }

    // 📋 Obtener todas las infracciones con estrategia GetAllType (All, Active, Deleted)
    public override async Task<IEnumerable<UserInfractionSelectDto>> GetAllAsync(GetAllType getAllType)
    {
        var strategy = GetStrategyFactory.GetStrategyGet(_repo, getAllType);
        var entities = await strategy.GetAll(_repo);
        return _mapper.Map<IEnumerable<UserInfractionSelectDto>>(entities);
    }

    // 🔎 Consultar infracciones por documento
    public async Task<IEnumerable<UserInfractionSelectDto>> GetByDocumentAsync(int documentTypeId, string documentNumber)
    {
        var entities = await _repo.GetByDocumentAsync(documentTypeId, documentNumber);
        return _mapper.Map<IReadOnlyList<UserInfractionSelectDto>>(entities);
    }

    // 🔎 Obtener infracción con datos completos para PDF
    public async Task<UserInfractionSelectDto> GetByIdAsyncPdf(int id)
    {
        try
        {
            var entity = await Data.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"InspectoraReport con ID {id} no encontrado.");
            }
            return _mapper.Map<UserInfractionSelectDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener InspectoraReport con ID {id}.");
            throw new BusinessException($"Error al obtener InspectoraReport con ID {id}.", ex);
        }
    }

    // ➕ Crear infracción normal (con userId conocido) + enviar correo con PDF
    public override async Task<UserInfractionDto> CreateAsync(UserInfractionDto dto)
    {
        // 🔹 NUEVO: Buscar el último SMLDV vigente antes de crear
        var currentSmldv = await _context.valueSmldv
            .Where(v => v.active && !v.is_deleted)
            .OrderByDescending(v => v.created_date)
            .FirstOrDefaultAsync()
            ?? throw new BusinessException("No hay SMLDV vigente registrado.");

        // 🔹 NUEVO: Guardar el valor histórico del SMLDV
        dto.smldvValueAtCreation = currentSmldv.value_smldv;

        // 🔹 NUEVO: Calcular amountToPay con el valor histórico
        var typeInfraction = await _types.GetByIdAsync(dto.typeInfractionId)
            ?? throw new BusinessException("Tipo de infracción inválido.");

        dto.amountToPay = typeInfraction.numer_smldv * dto.smldvValueAtCreation;

        // Resultado de la creación
        UserInfractionDto result = null!;

        // ----------------------------------------------------------------
        // Ejecutar la transacción DENTRO de la ExecutionStrategy (reintentos)
        // ----------------------------------------------------------------
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            // Abrimos la transacción dentro del ExecuteAsync para que EF Core pueda reintentar todo el bloque.
            await using (var trx = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Crear la infracción dentro de la transacción (mantengo tu llamada original)
                    result = await base.CreateAsync(dto);

                    // Confirmar la transacción
                    await trx.CommitAsync();
                }
                catch (Exception)
                {
                    try
                    {
                        await trx.RollbackAsync();
                    }
                    catch (Exception rbEx)
                    {
                        _logger.LogError(rbEx, "Error al hacer rollback de la transacción para la creación de infracción.");
                    }

                    throw; // propaga la excepción para que la estrategia la pueda manejar/reintentar si aplica
                }
            }
        });

        // ---------------------
        // Post-commit: encolar correo con PDF (mantengo exactamente tu lógica)
        // ---------------------    
        await _emailQueue.QueueBackgroundWorkItemAsync(async sp =>
        {
            using var scope = sp.CreateScope();

            var emailService = scope.ServiceProvider.GetRequiredService<IServiceEmail>();
            var pdfService = scope.ServiceProvider.GetRequiredService<IPdfGeneratorService>();
            var repo = scope.ServiceProvider.GetRequiredService<IPaymentAgreementRepository>();

            var agreement = await repo.GetByIdAsync(dto.userId);
            var dtoForPdf = _mapper.Map<PaymentAgreementSelectDto>(agreement);

            var pdfBytes = await pdfService.GeneratePaymentAgreementPdfAsync(dtoForPdf);
            var builder = new PaymentAgreementEmailBuilder(dtoForPdf, pdfBytes);

            var email = agreement.userInfraction?.User?.email;
            if (string.IsNullOrWhiteSpace(email))
                return;

            await emailService.SendEmailAsync(email, builder.GetSubject(), builder.GetBody());
        });


        // Post-commit: crear notificación del sistema y push realtime (en background) con scope propio
        _ = Task.Run(async () =>
        {
            _logger.LogInformation("NotificationTask: iniciando creación de notificación para infracción {InfractionId}", result.id);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var notifService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                var notificationDto = new NotificationCreateDto
                {
                    Title = "Multa creada",
                    Message = $"Hola, tu multa #{result.id:D6} fue registrada correctamente. Valor a pagar: {result.amountToPay:C}.",
                    Type = NotificationType.InfractionCreated,
                    Priority = NotificationPriority.Info,
                    RecipientUserId = dto.userId,
                    ActionRoute = "/infractions"
                };

                await notifService.CreateAsync(notificationDto);

                _logger.LogInformation("NotificationTask: notificación creada OK para infracción {InfractionId}", result.id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NotificationTask: error creando notificación para infracción {InfractionId}", result.id);
            }
        });

        // 🔹 🔹 🔹 ¡Esta línea faltaba!
        return result;
    }




    // 🚨 Nuevo método: Crear multa con datos de persona (cuando no hay User todavía)
    public async Task<UserInfractionSelectDto> CreateWithPersonAsync(CreateInfractionRequestDto dto)
    {
        // 1️⃣ Validar DTO
        var validator = new CreateInfractionRequestValidator();
        var validationResult = validator.Validate(dto);

        if (!validationResult.IsValid)
            throw new BusinessException(string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)));

        // 2️⃣ Buscar o crear usuario
        var user = await _users.FindByDocumentAsync(dto.DocumentTypeId, dto.DocumentNumber);

        if (user == null)
        {
            var person = new Person
            {
                firstName = dto.FirstName,
                lastName = dto.LastName,
                tipoUsuario = TipoUsuario.PersonaNormal
            };
            await _context.persons.AddAsync(person);
            await _context.SaveChangesAsync();

            user = new User
            {
                PersonId = person.id,
                documentTypeId = dto.DocumentTypeId,
                documentNumber = dto.DocumentNumber,
                email = dto.Email,
                PasswordHash = "DOC_LOGIN"
            };
            await _users.CreateAsync(user);
        }
        else
        {
            // actualizar correo si cambió
            if (!string.IsNullOrWhiteSpace(dto.Email) && user.email != dto.Email)
            {
                user.email = dto.Email;
                await _users.UpdateAsync(user);
            }
        }

        // 3️⃣ Validar tipo de infracción
        var typeInfraction = await _types.GetByIdAsync(dto.TypeInfractionId)
            ?? throw new BusinessException("Tipo de infracción inválido.");

        // 4️⃣ Obtener SMLDV vigente
        var smldv = await _context.valueSmldv
            .OrderByDescending(v => v.created_date)
            .FirstOrDefaultAsync()
            ?? throw new BusinessException("No hay SMLDV registrado.");

        // 5️⃣ Calcular monto
        var amount = dto.SmldvCount * smldv.value_smldv;

        // 6️⃣ Crear notificación inicial
        var notification = new UserNotification
        {
            message = $"Nueva infracción registrada: {typeInfraction.description}. Monto a pagar: {amount:C}",
            shippingDate = DateTime.UtcNow,
            active = true,
            is_deleted = false,
            created_date = DateTime.UtcNow
        };
        await _context.userNotification.AddAsync(notification);
        await _context.SaveChangesAsync();

        // 7️⃣ Crear infracción
        var infraction = new UserInfraction
        {
            UserId = user.id,
            InfractionId = dto.TypeInfractionId,
            dateInfraction = DateTime.UtcNow,
            stateInfraction = EstadoMulta.Pendiente,
            InformationFine = typeInfraction.description,
            amountToPay = amount,
            smldvValueAtCreation = smldv.value_smldv,
            UserNotificationId = notification.id
        };
        // ---------------------------------------------
        // ⭐ 8️⃣ Lógica dinámica de días/segundos con 5 recordatorios
        // ---------------------------------------------
        using var scope = _scopeFactory.CreateScope();
        var settingsService = scope.ServiceProvider.GetRequiredService<INotificationSettingServices>();
        var settings = (await settingsService.GetAllAsync()).ToList();

        // Unidad de tiempo: DAYS o SECONDS
        string timeUnit = settings.First().TimeUnit?.ToUpper() ?? "DAYS";
        bool usarSegundos = timeUnit == "SECONDS";

        // RANGOS DINÁMICOS
        int r1 = settings.Where(s => s.Days <= 10).OrderBy(s => s.Days).FirstOrDefault()?.Days ?? 3;
        int r2 = settings.Where(s => s.Days > 10 && s.Days <= 20).OrderBy(s => s.Days).FirstOrDefault()?.Days ?? 15;
        int r3 = settings.Where(s => s.Days > 20 && s.Days <= 30).OrderBy(s => s.Days).FirstOrDefault()?.Days ?? 25;
        int r4 = settings.Where(s => s.Days > 30 && s.Days <= 40).OrderBy(s => s.Days).FirstOrDefault()?.Days ?? 35;
        int r5 = settings.Where(s => s.Days > 40).OrderBy(s => s.Days).FirstOrDefault()?.Days ?? 45;

        DateTime fechaInf = infraction.dateInfraction;

        // Función dinámica para calcular fechas
        DateTime Calc(int v) => usarSegundos ? fechaInf.AddSeconds(v) : fechaInf.AddDays(v);

        // Guardar en la entidad
        infraction.paymentDue3Days = Calc(r1);
        infraction.paymentDue15Days = Calc(r2);
        infraction.paymentDue25Days = Calc(r3);
        infraction.paymentDue30Days = Calc(r4);
        infraction.paymentDue40Days = Calc(r5);

        // ---------------------------------------------

        await _repo.CreateAsync(infraction);

        // 9️⃣ Mapear DTO
        var infractionDto = _mapper.Map<UserInfractionSelectDto>(infraction);
        infractionDto.userEmail = user.email;

        await _context.SaveChangesAsync();

        // 🔟 Enviar correos en background (no bloqueante)
        await _scheduler.ScheduleEmailAsync(
             () => _emailOrchestrator.ProcesarNotificacionInicialAsync(infractionDto),
             TimeSpan.Zero
        );


        return infractionDto;
    }



    public async Task<IEnumerable<UserInfractionSelectDto>> GetByTypeInfractionAsync(int typeInfractionId)
    {
        var entities = await _repo.GetByTypeInfractionAsync(typeInfractionId);
        return _mapper.Map<IEnumerable<UserInfractionSelectDto>>(entities);
    }

    public async Task<UserInfractionSelectDto?> GetFirstByDocumentAsync(int documentTypeId, string documentNumber)
    {
        var entities = await _repo.GetByDocumentAsync(documentTypeId, documentNumber);
        var first = entities.OrderByDescending(u => u.dateInfraction).FirstOrDefault();
        return first != null ? _mapper.Map<UserInfractionSelectDto>(first) : null;
    }

    public async Task<bool> MarkAsPaidAsync(int infractionId)
    {
        var entity = await _repo.GetByIdAsync(infractionId);

        if (entity == null)
            throw new BusinessException($"La infracción {infractionId} no existe.");

        // Si ya está pagada, no hacer nada
        if (entity.stateInfraction == EstadoMulta.Pagada)
            return true;

        entity.stateInfraction = EstadoMulta.Pagada;
        entity.IsCoactive = false;
        entity.AccruedInterest = 0;
        entity.TotalToPay = 0;
        entity.amountToPay = 0;

        await _repo.UpdateAsync(entity);
        return true;
    }



    //public async Task<IEnumerable<UserInfractionSelectDto>> FilterAsync(UserInfractionFilterDto filter)
    //{
    //    var entities = await _repo.FilterAsync(filter);
    //    return _mapper.Map<IEnumerable<UserInfractionSelectDto>>(entities);
    //}

}