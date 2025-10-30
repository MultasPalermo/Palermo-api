using Business.Interfaces.IBusinessImplements.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web.WebBackgroundService
{
    public class PaymentAgreementBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<PaymentAgreementBackgroundService> _logger;

        // ⚠️ En Windows: "SA Pacific Standard Time"
        // ⚠️ En Linux:   "America/Bogota"
        private readonly TimeZoneInfo _tz =
            TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");

        public PaymentAgreementBackgroundService(
            IServiceProvider sp,
            ILogger<PaymentAgreementBackgroundService> logger)
        {
            _sp = sp;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando job diario de intereses/coactivo.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 1) Calcula próximo run a las 02:00 hora Bogotá
                    var nowUtc = DateTime.UtcNow;
                    var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, _tz);

                    var nextLocal = nowLocal.Date.AddHours(2); // 02:00
                    if (nowLocal >= nextLocal)
                        nextLocal = nextLocal.AddDays(1);

                    var delay = nextLocal - nowLocal;
                    _logger.LogInformation("Próxima ejecución en {delay} (local {nextLocal}).", delay, nextLocal);
                    await Task.Delay(delay, stoppingToken);

                    // 2) Ejecuta la lógica de negocio
                    using var scope = _sp.CreateScope();
                    var svc = scope.ServiceProvider.GetRequiredService<IPaymentAgreementServices>();

                    var affected = await svc.ApplyLateFeesAsync(DateTime.UtcNow, stoppingToken);
                    _logger.LogInformation("Intereses aplicados a {count} acuerdos.", affected);
                }
                catch (TaskCanceledException) { /* apagando */ }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error ejecutando el job de intereses.");
                    // Espera 1h y reintenta para no ciclar
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
            }
        }
    }
}
