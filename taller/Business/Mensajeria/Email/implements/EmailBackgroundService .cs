using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly EmailBackgroundQueue _queue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailBackgroundService> _logger;

        // 🔥 Configurar número de workers concurrentes
        private readonly int _maxConcurrentJobs;

        public EmailBackgroundService(
            EmailBackgroundQueue queue,
            IServiceProvider serviceProvider,
            ILogger<EmailBackgroundService> logger)
        {
            _queue = queue;
            _serviceProvider = serviceProvider;
            _logger = logger;

            // 🔥 AJUSTAR SEGÚN TUS NECESIDADES
            // Para pruebas locales: 10-20 workers
            // Para producción: 50-100 workers
            _maxConcurrentJobs = 20;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"🚀 EmailBackgroundService iniciado con {_maxConcurrentJobs} workers concurrentes");

            // 🔥 Crear múltiples workers que procesan en paralelo
            var workers = Enumerable.Range(0, _maxConcurrentJobs)
                .Select(workerId => ProcessQueueAsync(workerId, stoppingToken))
                .ToArray();

            try
            {
                await Task.WhenAll(workers);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("🛑 EmailBackgroundService detenido correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error crítico en EmailBackgroundService");
                throw;
            }
        }

        private async Task ProcessQueueAsync(int workerId, CancellationToken stoppingToken)
        {
            _logger.LogInformation($"👷 Worker #{workerId} iniciado");

            try
            {
                await foreach (var workItem in _queue.DequeueAsync(stoppingToken))
                {
                    try
                    {
                        _logger.LogDebug($"👷 Worker #{workerId} procesando job...");

                        // Crear scope de DI para servicios Scoped (DbContext, etc.)
                        using var scope = _serviceProvider.CreateScope();

                        // Ejecutar la tarea
                        await workItem();

                        _logger.LogDebug($"✅ Worker #{workerId} completó job exitosamente");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"❌ Worker #{workerId} falló al ejecutar job");
                        // No lanzar la excepción para que el worker continúe procesando
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"🛑 Worker #{workerId} detenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error crítico en Worker #{workerId}");
            }
        }
    }
}