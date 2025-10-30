using AutoMapper;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.PDF;
using Business.Mensajeria.Email.implements;
using Business.Mensajeria.Email.@interface;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Business.validaciones.Entities.PaymentAgreement;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.InstallmentSchedule;
using Entity.DTOs.Select.Entities;
using Entity.Infrastructure.Contexts;
using Entity.Init;
using FluentValidation;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Utilities.Exceptions;
// 👇 alias para evitar ambigüedad
using FVValidationException = FluentValidation.ValidationException;

namespace Business.Services.Entities
{
    public class PaymentAgreementServices
        : BusinessBasic<PaymentAgreementDto, PaymentAgreementSelectDto, PaymentAgreement>, IPaymentAgreementServices
    {
        private readonly ILogger<PaymentAgreementServices> _logger;
        private readonly IPaymentAgreementRepository _paymentAgreementRepository;
        private readonly ApplicationDbContext _context;
        private readonly EmailBackgroundQueue _emailQueue;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentAgreementServices(
            IPaymentAgreementRepository paymentAgreementRepository,
            IMapper mapper,
            ILogger<PaymentAgreementServices> logger,
            ApplicationDbContext context,
            EmailBackgroundQueue emailQueue,
            IServiceScopeFactory scopeFactory
        ) : base(paymentAgreementRepository, mapper, context)
        {
            _paymentAgreementRepository = paymentAgreementRepository;
            _logger = logger;
            _context = context;
            _emailQueue = emailQueue;
            _scopeFactory = scopeFactory;
        }


        public override async Task<IEnumerable<PaymentAgreementSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet(_paymentAgreementRepository, getAllType);
                var entities = await strategy.GetAll(_paymentAgreementRepository);
                return _mapper.Map<IEnumerable<PaymentAgreementSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public override async Task<PaymentAgreementSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"El acuerdo de pago con ID {id} no existe.");

                var entity = await _paymentAgreementRepository.GetByIdAsync(id);
                return _mapper.Map<PaymentAgreementSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        // Calcula la fecha final en función de la frecuencia y el número de cuotas
        private DateTime CalculateEndDateWithInstallments(DateTime startDate, string frequency, int installments)
        {
            if (installments <= 0)
                throw new BusinessException("La cantidad de cuotas debe ser mayor a cero.");

            var frequencyMap = new Dictionary<string, Func<DateTime, DateTime>>(StringComparer.OrdinalIgnoreCase)
            {
                { "MENSUAL", date => date.AddMonths(1) },
                { "QUINCENAL", date => date.AddDays(15) },
                { "BIMESTRAL", date => date.AddMonths(2) }
            };

            if (!frequencyMap.TryGetValue(frequency, out var nextDateFunc))
                throw new BusinessException($"Frecuencia de pago {frequency} no soportada.");

            var nextPaymentDate = startDate;
            for (int i = 0; i < installments; i++)
                nextPaymentDate = nextDateFunc(nextPaymentDate);

            return nextPaymentDate;
        }

        // ✅ Genera cronograma completo
        private List<InstallmentSchedule> GenerateInstallmentScheduleEntities(
     int paymentAgreementId,
     DateTime startDate,
     string frequency,
     int installments,
     decimal baseAmount,
     decimal monthlyFee)
        {
            if (installments <= 0)
                throw new BusinessException("El número de cuotas debe ser mayor a cero.");

            if (monthlyFee <= 0)
                throw new BusinessException("El valor de la cuota mensual debe ser mayor a cero.");

            var frequencyMap = new Dictionary<string, Func<DateTime, DateTime>>(StringComparer.OrdinalIgnoreCase)
    {
        { "MENSUAL", date => date.AddMonths(1) },
        { "QUINCENAL", date => date.AddDays(15) },
        { "BIMESTRAL", date => date.AddMonths(2) }
    };

            frequency = frequency.Trim().ToUpper();

            if (!frequencyMap.TryGetValue(frequency, out var nextDateFunc))
                throw new BusinessException($"Frecuencia de pago {frequency} no soportada.");

            var scheduleEntities = new List<InstallmentSchedule>();

            // ✅ Iniciar desde la SIGUIENTE fecha según la frecuencia
            var nextPaymentDate = nextDateFunc(startDate);
            var remaining = baseAmount;

            for (int i = 1; i <= installments; i++)
            {
                var cuota = (i == installments) ? remaining : monthlyFee;
                remaining -= cuota;

                scheduleEntities.Add(new InstallmentSchedule
                {
                    PaymentAgreementId = paymentAgreementId,
                    Number = i,
                    PaymentDate = nextPaymentDate,
                    Amount = cuota,
                    RemainingBalance = Math.Max(remaining, 0),
                    IsPaid = false
                });

                nextPaymentDate = nextDateFunc(nextPaymentDate);
            }

            return scheduleEntities;
        }


        public async Task<PaymentAgreementSelectDto> CreateAsync(PaymentAgreementDto dto)
        {
            // Crear el acuerdo en BD (ahora incluye el cronograma guardado)
            var entity = await CreatePaymentAgreementInternalAsync(dto);

            // Recargar con Include para traer el cronograma desde BD
            var agreementWithSchedule = await _paymentAgreementRepository.GetByIdAsync(entity.id);
            var resultDto = _mapper.Map<PaymentAgreementSelectDto>(agreementWithSchedule);

            _logger.LogInformation("Acuerdo {Id} creado con {Count} cuotas",
                resultDto.Id, resultDto.InstallmentSchedule?.Count ?? 0);

            // Envío de PDF y correo (background)
            await _emailQueue.QueueBackgroundWorkItemAsync(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IServiceEmail>();
                    var pdfService = scope.ServiceProvider.GetRequiredService<IPdfGeneratorService>();
                    var repository = scope.ServiceProvider.GetRequiredService<IPaymentAgreementRepository>();

                    // Consultar desde BD con el cronograma incluido
                    var agreement = await repository.GetByIdAsync(entity.id);
                    var dtoForPdf = _mapper.Map<PaymentAgreementSelectDto>(agreement);

                    _logger.LogInformation("Generando PDF con {Count} cuotas",
                        dtoForPdf.InstallmentSchedule?.Count ?? 0);

                    var pdfBytes = await pdfService.GeneratePaymentAgreementPdfAsync(dtoForPdf);
                    var builder = new PaymentAgreementEmailBuilder(dtoForPdf, pdfBytes);

                    var email = agreement.userInfraction?.User?.email;
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        _logger.LogWarning("Usuario sin correo para acuerdo {Id}", entity.id);
                        return;
                    }

                    await emailService.SendEmailAsync(
                        email,
                        builder.GetSubject(),
                        builder.GetBody(),
                        builder.GetAttachments()
                    );

                    _logger.LogInformation("Correo enviado correctamente para acuerdo {Id}", entity.id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error enviando correo con acuerdo {Id}", entity.id);
                }
            });

            return resultDto;
        }

        // Método interno para la creación de PaymentAgreement
        private async Task<PaymentAgreement> CreatePaymentAgreementInternalAsync(PaymentAgreementDto dto)
        {
            BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

            var createValidator = new PaymentAgreementDtoValidator<PaymentAgreementDto>();
            var validationResult = createValidator.Validate(dto);
            if (!validationResult.IsValid)
                throw new FVValidationException(validationResult.Errors);

            var userInfraction = await _paymentAgreementRepository.GetUserInfractionWithDetailsAsync(dto.userInfractionId);
            if (userInfraction == null)
                throw new BusinessException($"La infracción con ID {dto.userInfractionId} no existe.");

            if (userInfraction.stateInfraction != EstadoMulta.Pendiente)
                throw new BusinessException($"La infracción con ID {dto.userInfractionId} no permite acuerdos de pago porque está en estado {userInfraction.stateInfraction}.");

            var frequency = await _paymentAgreementRepository.GetPaymentFrequencyAsync(dto.paymentFrequencyId)
                ?? throw new BusinessException($"La frecuencia de pago con ID {dto.paymentFrequencyId} no existe.");

            var typePayment = await _paymentAgreementRepository.GetTypePaymentAsync(dto.typePaymentId)
                ?? throw new BusinessException($"El método de pago con ID {dto.typePaymentId} no existe.");

            var (baseAmount, installments, monthlyFee) = CalcularMontos(userInfraction, dto);

            var startDate = DateTime.Now.Date;
            var endDate = CalculateEndDateWithInstallments(startDate, frequency.intervalPage, installments);

            var agreement = new PaymentAgreement
            {
                AgreementStart = startDate,
                AgreementEnd = endDate,
                expeditionCedula = dto.expeditionCedula,
                userInfractionId = dto.userInfractionId,
                paymentFrequencyId = dto.paymentFrequencyId,
                typePaymentId = dto.typePaymentId,
                address = dto.address,
                neighborhood = dto.neighborhood,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                AgreementDescription = dto.AgreementDescription
                    ?? $"Acuerdo para {userInfraction.User.Person?.firstName} {userInfraction.User.Person?.lastName} - Infracción: {userInfraction.Infraction.description}",
                BaseAmount = baseAmount,
                AccruedInterest = 0m,
                OutstandingAmount = baseAmount,
                IsPaid = false,
                IsCoactive = false,
                Installments = installments,
                MonthlyFee = monthlyFee
            };

            userInfraction.stateInfraction = EstadoMulta.ConAcuerdoPago;
            _context.userInfraction.Update(userInfraction);

            var created = await _paymentAgreementRepository.CreateAsync(agreement);
            await _context.SaveChangesAsync();

            // Generar y guardar el cronograma en la base de datos
            var cronogramaEntities = GenerateInstallmentScheduleEntities(
                created.id,
                startDate,
                frequency.intervalPage,
                installments,
                baseAmount,
                monthlyFee
            );

            _context.Set<InstallmentSchedule>().AddRange(cronogramaEntities);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Cronograma de {Count} cuotas guardado en BD para acuerdo {Id}",
                cronogramaEntities.Count, created.id);

            return created;
        }

        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                if (!await ExistsAsync(id))
                    throw new BusinessException($"No se puede restaurar. El acuerdo de pago con ID {id} no existe.");

                return await base.RestoreLogical(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al restaurar el registro con ID {id}.", ex);
            }
        }

        public async Task<int> ApplyLateFeesAsync(DateTime nowUtc, CancellationToken ct = default)
        {
            int updated = 0;
            DateTime today = nowUtc.Date;

            var agreements = await _context.paymentAgreement
                .Where(a => !a.is_deleted && !a.IsPaid)
                .ToListAsync(ct);

            foreach (var a in agreements)
            {
                DateTime coactiveDate = a.AgreementStart.Date.AddDays(30);

                if (today >= coactiveDate && !a.IsCoactive)
                {
                    a.IsCoactive = true;
                    a.CoactiveActivatedOn = coactiveDate;
                    a.LastInterestAppliedOn = coactiveDate.AddDays(-1);
                }

                if (a.IsCoactive)
                {
                    DateTime lastApplied = a.LastInterestAppliedOn?.Date
                        ?? a.CoactiveActivatedOn!.Value.AddDays(-1);

                    int daysToAccrue = (today - lastApplied).Days;

                    if (daysToAccrue > 0)
                    {
                        decimal monthlyRate = 0.02m;
                        int divisor = 30;
                        decimal dailyRate = monthlyRate / divisor;

                        decimal interestToAdd = a.OutstandingAmount * dailyRate * daysToAccrue;

                        a.AccruedInterest += interestToAdd;
                        a.OutstandingAmount = a.BaseAmount + a.AccruedInterest;
                        a.LastInterestAppliedOn = today;

                        updated++;
                    }
                }
            }

            if (updated > 0)
                await _context.SaveChangesAsync(ct);

            return updated;
        }

        public async Task<IEnumerable<PaymentAgreementInitDto>> GetInitDataAsync(int userId, int? infractionId = null)
        {
            return await _paymentAgreementRepository.GetInitDataAsync(userId, infractionId);
        }

        public (decimal BaseAmount, int Installments, decimal MonthlyFee) CalcularMontos(
            UserInfraction userInfraction,
            PaymentAgreementDto dto)
        {
            var detail = userInfraction.Infraction.fineCalculationDetail
                .OrderByDescending(fd => fd.valueSmldv.Current_Year)
                .FirstOrDefault();

            if (detail == null)
                throw new BusinessException("No existe detalle de cálculo para esta infracción.");

            decimal baseAmount = userInfraction.Infraction.numer_smldv * (decimal)detail.valueSmldv.value_smldv;
            int installments = dto.Installments > 0 ? dto.Installments.Value : 1;
            decimal monthlyFee = Math.Round(baseAmount / installments, 0, MidpointRounding.AwayFromZero);

            return (baseAmount, installments, monthlyFee);
        }


        public Task<PaymentAgreementSelectDto> GetByIdAsyncPdf(int id)
        {
            throw new NotImplementedException();
        }
    }
}
