using AutoMapper;
using Business.Services;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.Infrastructure.Contexts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web.Hubs;

public class InfractionDiscountBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InfractionDiscountBackgroundService> _logger;
    private readonly IMapper _mapper;

    public InfractionDiscountBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<InfractionDiscountBackgroundService> logger,
        IMapper mapper)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _mapper = mapper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var discountService = scope.ServiceProvider.GetRequiredService<DiscountService>();
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<InfractionHub>>();

                // Traer todas las infracciones activas
                var activeInfractions = await dbContext.userInfraction
                    .Where(i => i.stateInfraction == EstadoMulta.Pendiente)
                    .ToListAsync(stoppingToken);

                foreach (var infraction in activeInfractions)
                {
                    var infractionDto = _mapper.Map<UserInfractionDto>(infraction);

                    // Buscar tipo de infracción
                    var typeInfraction = await dbContext.Infraction
                    .Include(i => i.TypeInfraction)  // 🔹 incluir la relación
                    .FirstOrDefaultAsync(t => t.id == infraction.InfractionId, stoppingToken);

                    if (typeInfraction == null)
                    {
                        _logger.LogWarning("⚠️ No existe TypeInfraction con id {id}", infraction.InfractionId);
                        continue;
                    }

                    // Buscar valor SMLDV vigente
                    var smldv = await dbContext.valueSmldv
                        .OrderByDescending(v => v.Current_Year)
                        .FirstOrDefaultAsync(stoppingToken);

                    if (smldv == null)
                    {
                        _logger.LogWarning("⚠️ No se encontró un valor SMLDV vigente");
                        continue;
                    }

                    // Calcular monto base
                    decimal baseAmount = typeInfraction.numer_smldv * (decimal)smldv.value_smldv;

                    // Calcular descuento con el servicio
                    var detailDto = discountService.Calculate(
                        infractionDto,
                        baseAmount,
                        smldv.id,
                        $"SMLDV {smldv.Current_Year}",
                        typeInfraction.TypeInfraction.Name // 🔹 ahora sí existe
                    );


                    // 📝 Log para verificar el cálculo
                    int daysPassed = (DateTime.Now.Date - infraction.dateInfraction.Date).Days;
                    _logger.LogInformation(
                        "📌 Infracción {InfractionId}: {DaysPassed} días transcurridos → {Percentaje:P0} aplicado. Base {BaseAmount:C}, Total {Total:C}",
                        infraction.id,
                        daysPassed,
                        detailDto.percentaje,
                        baseAmount,
                        detailDto.totalCalculation
                    );

                    if (string.IsNullOrWhiteSpace(detailDto.formula))
                    {
                        detailDto.formula = $"Base {baseAmount} - {detailDto.percentaje * 100}% descuento";
                    }

                    // Mapear DTO → Entidad
                    var detailEntity = _mapper.Map<FineCalculationDetail>(detailDto);
                    detailEntity.typeInfractionId = typeInfraction.id;
                    detailEntity.valueSmldvId = smldv.id;

                    dbContext.fineCalculationDetail.Add(detailEntity);

                    // Actualizar monto a pagar en UserInfraction
                    infraction.amountToPay = detailDto.totalCalculation;
                    dbContext.userInfraction.Update(infraction);
                }

                await dbContext.SaveChangesAsync(stoppingToken);

                await hubContext.Clients.All.SendAsync("InfractionsUpdated", DateTime.Now);

                _logger.LogInformation("✅ Descuentos aplicados a las infracciones {time}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error aplicando descuentos");
            }

            // ✅ Modo normal: ejecutar cada 24 horas
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }
    }
}
