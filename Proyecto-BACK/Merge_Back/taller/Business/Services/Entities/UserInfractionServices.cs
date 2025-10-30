using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.PDF;
using Business.Mensajeria.Email.implements;
using Business.Mensajeria.Email.@interface;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Business.Validaciones.Entities.UserInfraction;
using Data.Interfaces.IDataImplement.Entities;   // <- IUserInfractionRepository
using Data.Interfaces.IDataImplement.Security;   // <- IUserRepository
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.AnexarMulta;           // <- DTO especial para anexar multas con persona
using Entity.DTOs.Default.EntitiesDto;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using SendGrid.Helpers.Errors.Model;
using Utilities.Exceptions;

public class UserInfractionServices
    : BusinessBasic<UserInfractionDto, UserInfractionSelectDto, UserInfraction>, IUserInfractionServices
{
    private readonly ILogger<UserInfractionServices> _logger;
    private readonly IUserInfractionRepository _repo;       // CRUD principal de infracciones
    private readonly IUserRepository _users;                 // FK -> User
    private readonly IInfractionRepository _types;      // FK -> Tipo de infracción
    private readonly IUserNotificationRepository _notifs;   // FK -> Notificaciones de usuario
    private readonly EmailBackgroundQueue _emailQueue;      // Cola para enviar correos en background
    private readonly IServiceScopeFactory _scopeFactory;    // Permite crear servicios scoped en tareas background
    private readonly IPdfGeneratorService _pdfservices;     // Servicio para generar PDFs

    public UserInfractionServices(
        IUserInfractionRepository repo,
        IUserRepository users,
        IInfractionRepository types,
        IUserNotificationRepository notifs,
        IMapper mapper,
        ILogger<UserInfractionServices> logger,
        EmailBackgroundQueue emailQueue,
        IServiceScopeFactory scopeFactory,
        Entity.Infrastructure.Contexts.ApplicationDbContext db,
        IPdfGeneratorService pdfService
    ) : base(repo, mapper, db)
    {
        _repo = repo;
        _users = users;
        _types = types;
        _notifs = notifs;
        _logger = logger;
        _emailQueue = emailQueue;
        _scopeFactory = scopeFactory;
        _pdfservices = pdfService;
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

        // Crear la infracción
        var result = await base.CreateAsync(dto);

        // 📨 Enviar correo con PDF de la multa
        await _emailQueue.QueueBackgroundWorkItemAsync(async () =>
        {
            using var scope = _scopeFactory.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<IServiceEmail>();
            var pdfService = scope.ServiceProvider.GetRequiredService<IPdfGeneratorService>();

            var user = await _users.GetByIdAsync(dto.userId);

            // Generamos el PDF con los datos de la infracción recién creada
            var pdfBytes = await pdfService.GeneratePdfAsync(await GetByIdAsync(result.id));

            var builder = new InfraccionEmailBuilder(
                await GetByIdAsync(result.id),
                pdfBytes
            );

            await emailService.SendEmailAsync(
                user!.email,
                builder.GetSubject(),
                builder.GetBody()
            );
        });

        return result;
    }

    // 🚨 Nuevo método: Crear multa con datos de persona (cuando no hay User todavía)
    public async Task<UserInfractionSelectDto> CreateWithPersonAsync(CreateInfractionRequestDto dto)
    {
        // Validar el DTO
        var validator = new CreateInfractionRequestValidator();
        var validationResult = validator.Validate(dto);

        if (!validationResult.IsValid)
            throw new BusinessException(
                string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage))
            );

        // 1. Buscar si ya existe el usuario por documento
        var user = await _users.FindByDocumentAsync(dto.DocumentTypeId, dto.DocumentNumber);

        if (user == null)
        {
            // Crear Persona
            var person = new Person
            {
                firstName = dto.FirstName,
                lastName = dto.LastName,
                municipalityId = null,
                phoneNumber = null,
                address = null,
                tipoUsuario = TipoUsuario.PersonaNormal
            };
            await _context.persons.AddAsync(person);
            await _context.SaveChangesAsync();

            // Crear Usuario asociado a la persona
            user = new User
            {
                PersonId = person.id,
                documentTypeId = dto.DocumentTypeId,
                documentNumber = dto.DocumentNumber,
                email = dto.Email,
                PasswordHash = "DOC_LOGIN" // marcador para login por documento
            };
            await _users.CreateAsync(user);
        }

        // 2. Validar tipo de infracción
        var typeInfraction = await _types.GetByIdAsync(dto.TypeInfractionId)
            ?? throw new BusinessException("Tipo de infracción inválido.");

        // 3. Buscar el último SMLDV vigente
        var smldv = await _context.valueSmldv
            .OrderByDescending(v => v.created_date)
            .FirstOrDefaultAsync()
            ?? throw new BusinessException("No hay SMLDV registrado.");

        // 4. Calcular monto a pagar directamente
        var amount = dto.SmldvCount * smldv.value_smldv;

        // 5. Crear notificación asociada
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

        // 6. Crear UserInfraction
        var infraction = new UserInfraction
        {
            UserId = user.id,
            InfractionId = dto.TypeInfractionId,
            dateInfraction = DateTime.UtcNow,
            stateInfraction = EstadoMulta.Pendiente,
            InformationFine = typeInfraction.description,
            amountToPay = amount,
            smldvValueAtCreation = smldv.value_smldv, // Guardamos el valor histórico
            UserNotificationId = notification.id
        };

        await _repo.CreateAsync(infraction);

        // 🔄 Mapear a DTO completo (con los datos cargados)
        var infractionDto = _mapper.Map<UserInfractionSelectDto>(infraction);

        // 📄 Generar PDF
        var pdfBytes = await _pdfservices.GeneratePdfAsync(infractionDto);

        // 📨 Enviar correo en background con PDF adjunto
        await _emailQueue.QueueBackgroundWorkItemAsync(async () =>
        {
            using var scope = _scopeFactory.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<IServiceEmail>();

            var builder = new InfraccionEmailBuilder(infractionDto, pdfBytes);

            await emailService.SendEmailAsync(
                user.email,
                builder.GetSubject(),
                builder.GetBody(),
                builder.GetAttachments()?.ToList()
            );
        });

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



    //public async Task<IEnumerable<UserInfractionSelectDto>> FilterAsync(UserInfractionFilterDto filter)
    //{
    //    var entities = await _repo.FilterAsync(filter);
    //    return _mapper.Map<IEnumerable<UserInfractionSelectDto>>(entities);
    //}

}