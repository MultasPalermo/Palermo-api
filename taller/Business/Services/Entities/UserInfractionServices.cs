using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.IBusinessImplements.parameters;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Interfaces.Notificacion;
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
using Data.Services.Security;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.AnexarMulta;           // <- DTO especial para anexar multas con persona
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Default.Notificacion;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using SendGrid.Helpers.Errors.Model;
using System;
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
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;
    private readonly EmailScheduler _scheduler;
    private readonly EmailOrchestrator _emailOrchestrator;
    private readonly IServiceProvider _serviceProvider;
    private readonly IInfractionDiscountRunner _discountRunner;

    public UserInfractionServices(
        IUserInfractionRepository repo,
        IUserRepository users,
        IInfractionRepository types,
        IUserNotificationRepository notifs,
        IMapper mapper,
        ILogger<UserInfractionServices> logger,
        EmailBackgroundQueue emailQueue,
        EmailScheduler scheduler,
        EmailOrchestrator emailOrchestrator,
        IServiceScopeFactory scopeFactory,
        Entity.Infrastructure.Contexts.ApplicationDbContext db,
        IPdfGeneratorService pdfService,
        INotificationService notificationService,
        IServiceProvider serviceProvider,
        IInfractionDiscountRunner discountRunner
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
        _scheduler = scheduler;
        _notificationService = notificationService;
        _emailOrchestrator = emailOrchestrator;
        _serviceProvider = serviceProvider;
        _discountRunner = discountRunner;// <-- asignación
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
        await _emailQueue.QueueBackgroundWorkItemAsync(async () =>
        {
            using var scope = _scopeFactory.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<IServiceEmail>();
            var pdfService = scope.ServiceProvider.GetRequiredService<IPdfGeneratorService>();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserService>();
            var infractionRepo = scope.ServiceProvider.GetRequiredService<IUserInfractionServices>();

            // Traer el usuario y la infracción desde el SCOPE CORRECTO
            var user = await userRepo.GetByIdAsync(dto.userId);
            var infraction = await infractionRepo.GetByIdAsync(result.id);

            // Generar el PDF dentro del scope
            var pdfBytes = await pdfService.GeneratePdfAsync(infraction);

            var builder = new InfraccionEmailBuilder(
                infraction,
                pdfBytes
            );

            await emailService.SendEmailAsync(
                user!.email,
                builder.GetSubject(),
                builder.GetBody()
            );
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
        // 1. validación
        var validator = new CreateInfractionRequestValidator();
        var validationResult = validator.Validate(dto);

        if (!validationResult.IsValid)
            throw new BusinessException(string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)));

        // 2. buscar o crear usuario
        var user = await _users.FindByDocumentAsync(dto.DocumentTypeId, dto.DocumentNumber);

        if (user == null)
        {
            var person = new Person
            {
                firstName = dto.FirstName,
                lastName = dto.LastName,
                tipoUsuario = TipoUsuario.PersonaNormal
            };

            // Evitar warning CS8602 usando null-forgiving en _context
            await _context!.persons.AddAsync(person);
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
            if (!string.IsNullOrWhiteSpace(dto.Email) && user.email != dto.Email)
            {
                user.email = dto.Email;
                await _users.UpdateAsync(user);
            }
        }

        // 3. validar tipo de infracción
        var typeInfraction = await _types.GetByIdAsync(dto.TypeInfractionId)
            ?? throw new BusinessException("tipo de infracción inválido");

        // 4. obtener smldv vigente (usar null-forgiving en _context para evitar CS8602)
        var smldv = await _context!.valueSmldv
            .OrderByDescending(v => v.created_date)
            .FirstOrDefaultAsync()
            ?? throw new BusinessException("no hay smldv registrado");

        var baseAmount = dto.SmldvCount * smldv.value_smldv;

        // 5. crear notificación inicial
        var notification = new UserNotification
        {
            message = $"nueva infracción registrada: {typeInfraction.description}. monto base: {baseAmount:C}",
            shippingDate = DateTime.UtcNow,
            active = true,
            is_deleted = false,
            created_date = DateTime.UtcNow
        };

        await _context.userNotification.AddAsync(notification);
        await _context.SaveChangesAsync();

        // 6. crear infracción
        var infraction = new UserInfraction
        {
            UserId = user.id,
            InfractionId = dto.TypeInfractionId,
            dateInfraction = DateTime.UtcNow,
            stateInfraction = EstadoMulta.Pendiente,
            InformationFine = typeInfraction.description,

            // inicia con el valor base (sin descuento aplicado en esta capa)
            amountToPay = baseAmount,
            InitialAmount = baseAmount,
            TotalToPay = baseAmount,

            smldvValueAtCreation = smldv.value_smldv,
            UserNotificationId = notification.id,

            StatusCollection = EstadoCobro.CobroPrejuridico,

            IsCoactive = false,
            CoactiveActivatedOn = null,
            LastInterestAppliedOn = null,
            AccruedInterest = 0,
            DaysOfDelay = 0
        };

        await _repo.CreateAsync(infraction);
        await _context.SaveChangesAsync();

        var infractionDto = _mapper.Map<UserInfractionSelectDto>(infraction);
        infractionDto.userEmail = user.email ?? string.Empty;

        // 🚀 CORRECCIÓN CLAVE: Clonar el record (usando 'with {}') para 'dtoParaR0'.
        // Esto previene que la tarea asíncrona de notificación inicial capture una referencia
        // que podría ser sobrescrita por una segunda multa creada rápidamente.
        var dtoParaR0 = infractionDto with { };

        string jobIdInicial = $"Infraction_{dtoParaR0.id}_Status_{EstadoCobro.CobroPrejuridico}";

        await _scheduler.ScheduleEmailAsync(
            () => _emailOrchestrator.ProcesarNotificacionInicialAsync(dtoParaR0),
            TimeSpan.Zero,
            jobIdInicial
        );


        var reminderService = _scopeFactory.CreateScope()
            .ServiceProvider
            .GetRequiredService<ReminderEmailAppService>();


        // ProgramarRecordatoriosAsync ya contiene su propia clonación defensiva.
        await reminderService.ProgramarRecordatoriosAsync(infractionDto);

        try
        {
            // 7. Ejecutar descuento inicial
            await _discountRunner.RunOnceFor(infraction.id);

            _logger.LogInformation("✅ Ejecución RunOnceFor completada con éxito. Descuento aplicado.");

            var updated = await _context.userInfraction
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id == infraction.id);

            if (updated != null)
            {
                // Re-mapear el DTO de salida con la entidad que ya contiene el descuento.
                infractionDto = _mapper.Map<UserInfractionSelectDto>(updated);
                infractionDto.userEmail = user.email ?? string.Empty;

                _logger.LogInformation($"📝 Infracción #{infraction.id} recargada. Monto final con descuento: {infractionDto.amountToPay:C}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al ejecutar el RunOnceFor para aplicar el descuento inicial.");
        }

        return infractionDto;
    }


    public async Task<int> ApplyInterestToInfractionsAsync(DateTime nowUtc, CancellationToken ct = default)                     
    {
        int updated = 0;
        DateTime today = nowUtc.Date;

        var infractions = await _context.userInfraction
            .Where(i => !i.is_deleted && i.stateInfraction == EstadoMulta.Pendiente)
            .ToListAsync(ct);

        foreach (var i in infractions)
        {
            // Calcular días de mora siempre
            i.DaysOfDelay = (today - i.dateInfraction.Date).Days;
            if (i.DaysOfDelay < 0) i.DaysOfDelay = 0;

            // Activar coactivo día 30
            DateTime coactiveDate = i.dateInfraction.Date.AddDays(30);

            if (today >= coactiveDate && !i.IsCoactive)
            {
                i.IsCoactive = true;
                i.CoactiveActivatedOn = coactiveDate;
                i.LastInterestAppliedOn = coactiveDate.AddDays(-1);
            }

            if (i.IsCoactive)
            {
                DateTime lastApplied = i.LastInterestAppliedOn?.Date
                    ?? i.CoactiveActivatedOn!.Value.AddDays(-1);

                int daysToAccrue = (today - lastApplied).Days;

                if (daysToAccrue > 0)
                {
                    decimal monthlyRate = 0.02m;
                    decimal dailyRate = monthlyRate / 30;

                    decimal interestToAdd = i.InitialAmount * dailyRate * daysToAccrue;

                    i.AccruedInterest += interestToAdd;
                    i.LastInterestAppliedOn = today;

                    updated++;
                }
            }

            // Calcular total
            i.TotalToPay = i.InitialAmount + i.AccruedInterest;

            // Mantener sincronizado con amountToPay
            i.amountToPay = i.TotalToPay;
        }

        if (updated > 0)
            await _context.SaveChangesAsync(ct);

        return updated;
    }

    public async Task<bool> ApplyInterestToSingleInfractionAsync(
    int idUserInfraction,
    DateTime simulatedNowUtc,
    CancellationToken ct = default)
    {
        DateTime today = simulatedNowUtc.Date;

        var i = await _context.userInfraction
            .FirstOrDefaultAsync(x =>
                x.id == idUserInfraction &&
                !x.is_deleted &&
                x.stateInfraction == EstadoMulta.Pendiente,
                ct);

        if (i == null)
            return false;

        // recalcular días de mora con la fecha simulada
        i.DaysOfDelay = (today - i.dateInfraction.Date).Days;
        if (i.DaysOfDelay < 0) i.DaysOfDelay = 0;

        // activar coactivo si ya pasaron 30 días
        DateTime coactiveDate = i.dateInfraction.Date.AddDays(30);

        if (today >= coactiveDate && !i.IsCoactive)
        {
            i.IsCoactive = true;
            i.CoactiveActivatedOn = coactiveDate;
            i.LastInterestAppliedOn = coactiveDate.AddDays(-1);
        }

        if (i.IsCoactive)
        {
            DateTime lastApplied = i.LastInterestAppliedOn?.Date
                ?? i.CoactiveActivatedOn!.Value.AddDays(-1);

            int daysToAccrue = (today - lastApplied).Days;

            if (daysToAccrue > 0)
            {
                decimal monthlyRate = 0.02m;
                decimal dailyRate = monthlyRate / 30;

                decimal interestToAdd = i.InitialAmount * dailyRate * daysToAccrue;

                i.AccruedInterest += interestToAdd;
                i.LastInterestAppliedOn = today;
            }
        }

        // total
        i.TotalToPay = i.InitialAmount + i.AccruedInterest;
        i.amountToPay = i.TotalToPay;

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IEnumerable<UserInfractionSelectDto>> GetMultasAsync(
    int? documentTypeId,
    int? typeInfractionId,
    EstadoMulta? stateInfraction)
    {
        try
        {
            var result = await _repo.GetMultasAsync(
                documentTypeId,
                typeInfractionId,
                stateInfraction
            );

            return _mapper.Map<IEnumerable<UserInfractionSelectDto>>(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR en UserInfractionService] {ex.Message}");
            throw new Exception("Error en la capa de negocio al consultar multas", ex);
        }
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



    //public async Task<IEnumerable<UserInfractionSelectDto>> FilterAsync(UserInfractionFilterDto filter)
    //{
    //    var entities = await _repo.FilterAsync(filter);
    //    return _mapper.Map<IEnumerable<UserInfractionSelectDto>>(entities);
    //}

}