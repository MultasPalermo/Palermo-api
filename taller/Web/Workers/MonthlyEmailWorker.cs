using Business.Mensajeria.Email.implements; // MonthlyEmailAppService
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web.Workers
{
    public class MonthlyEmailWorker : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<MonthlyEmailWorker> _logger;
        private readonly TimeZoneInfo _tz;
        private readonly int _hour;
        private readonly int _minute;

        public MonthlyEmailWorker(IServiceProvider sp, ILogger<MonthlyEmailWorker> logger, IConfiguration cfg)
        {
            _sp = sp;
            _logger = logger;

            // Linux: "America/Bogota" | Windows: "SA Pacific Standard Time"
            var tzId = cfg["Scheduler:TzId"] ?? "America/Bogota";
            try { _tz = TimeZoneInfo.FindSystemTimeZoneById(tzId); }
            catch { _tz = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"); }

            _hour = int.TryParse(cfg["Scheduler:Hour"], out var h) ? h : 8;
            _minute = int.TryParse(cfg["Scheduler:Minute"], out var m) ? m : 0;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            _logger.LogInformation("MonthlyEmailWorker iniciado.");

            while (!ct.IsCancellationRequested)
            {
                var nowLocal = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _tz);
                var nextRun = ComputeNextRun(nowLocal);

                _logger.LogInformation("Próxima ejecución programada: {NextRun}", nextRun);

                var delay = nextRun - nowLocal;
                if (delay < TimeSpan.Zero) delay = TimeSpan.Zero;

                try
                {
                    await Task.Delay(delay, ct);

                    // Doble verificación: día 4 y ventana exacta (hora:minuto hasta +1 min)
                    var check = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _tz);
                    var target = new DateTimeOffset(check.Year, check.Month, check.Day, _hour, _minute, 0, check.Offset);

                    if (check.Day == 4 && check >= target && check < target.AddMinutes(1))
                    {
                        await RunJobAsync(ct);
                    }
                    else
                    {
                        _logger.LogInformation("Saltado: no estamos en la ventana del día 4 ({NowLocal})", check);
                    }
                }
                catch (TaskCanceledException)
                {
                    // apagando
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fallo en scheduler mensual, reintentando en 1h.");
                    await Task.Delay(TimeSpan.FromHours(1), ct);
                }
            }

            _logger.LogInformation("MonthlyEmailWorker detenido.");
        }

        // Próximo disparo: día 4 a la hora/minuto configurados
        private DateTimeOffset ComputeNextRun(DateTimeOffset nowLocal)
        {
            var targetToday = new DateTimeOffset(
                nowLocal.Year, nowLocal.Month, nowLocal.Day, _hour, _minute, 0, nowLocal.Offset);

            // Si hoy es 4 y aún no pasó la hora -> hoy
            if (nowLocal.Day == 4 && nowLocal <= targetToday)
                return targetToday;

            // Si estamos antes del 4 de este mes -> programa este mes día 4
            if (nowLocal.Day < 4)
            {
                var thisMonthFourth = new DateTimeOffset(nowLocal.Year, nowLocal.Month, 4, _hour, _minute, 0, nowLocal.Offset);
                return thisMonthFourth;
            }

            // Si ya pasó el 4 -> siguiente mes día 4
            var nextYear = nowLocal.Month == 12 ? nowLocal.Year + 1 : nowLocal.Year;
            var nextMonth = nowLocal.Month == 12 ? 1 : nowLocal.Month + 1;
            return new DateTimeOffset(nextYear, nextMonth, 4, _hour, _minute, 0, nowLocal.Offset);
        }

        private async Task RunJobAsync(CancellationToken ct)
        {
            // Guardia extra: evita ejecutar si por alguna razón no es día 4
            var nowLocal = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _tz);
            if (nowLocal.Day != 4)
            {
                _logger.LogInformation("Abortado: hoy no es día 4 ({Now})", nowLocal);
                return;
            }

            using var scope = _sp.CreateScope();
            var app = scope.ServiceProvider.GetRequiredService<MonthlyEmailAppService>();

            _logger.LogInformation("Ejecutando envío mensual (día 4)...");
            await app.EjecutarEnvioMensualAsync(ct);
            _logger.LogInformation("Envío mensual completado.");
        }
    }
}
