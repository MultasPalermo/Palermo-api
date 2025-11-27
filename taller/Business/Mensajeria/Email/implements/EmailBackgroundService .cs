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
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailBackgroundService> _logger;
        private readonly int _maxConcurrentJobs;

        public EmailBackgroundService(
            EmailBackgroundQueue queue,
            IServiceScopeFactory scopeFactory,
            ILogger<EmailBackgroundService> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;   // ← cambia esto
            _logger = logger;
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
            _logger.LogInformation($"worker {workerId} iniciado");

            try
            {
                await foreach (var workItem in _queue.DequeueAsync(stoppingToken))
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope(); // ← cambia esto

                        var scopedProvider = scope.ServiceProvider;

                        await workItem(scopedProvider); // ← pásale el provider
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"worker {workerId} falló");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"worker {workerId} detenido");
            }
        }
    }
}