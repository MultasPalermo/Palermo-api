using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class EmailScheduler : IReminderScheduler
    {
        private readonly EmailBackgroundQueue _queue;
        private readonly ILogger<EmailScheduler> _logger;

        // 🔥 CAMBIO 1: Usar un identificador único por job
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _scheduledJobs;

        public EmailScheduler(EmailBackgroundQueue queue, ILogger<EmailScheduler> logger)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scheduledJobs = new ConcurrentDictionary<string, CancellationTokenSource>();
        }

        public async Task ScheduleEmailAsync(Func<Task> sendEmailFunc, TimeSpan delay, string jobId)
        {
            if (sendEmailFunc == null)
                throw new ArgumentNullException(nameof(sendEmailFunc));
            if (string.IsNullOrWhiteSpace(jobId))
                throw new ArgumentNullException(nameof(jobId));

            // 🔥 CAMBIO 2: Generar un ID único interno si ya existe
            var internalJobId = jobId;
            var attempt = 0;

            while (_scheduledJobs.ContainsKey(internalJobId) && attempt < 100)
            {
                attempt++;
                internalJobId = $"{jobId}_{attempt}";
                _logger.LogWarning($"⚠️ Job '{jobId}' ya existe. Generando ID único: '{internalJobId}'");
            }

            // Crear la fuente de cancelación
            var cts = new CancellationTokenSource();

            // 🔥 CAMBIO 3: Si falla el TryAdd después de 100 intentos, usar AddOrUpdate
            if (!_scheduledJobs.TryAdd(internalJobId, cts))
            {
                _logger.LogWarning($"⚠️ No se pudo agregar '{internalJobId}'. Usando AddOrUpdate como fallback.");

                var oldCts = _scheduledJobs.AddOrUpdate(
                    internalJobId,
                    cts,
                    (key, existingCts) =>
                    {
                        existingCts?.Cancel();
                        existingCts?.Dispose();
                        return cts;
                    }
                );
            }

            _logger.LogInformation($"📅 Job '{internalJobId}' programado para ejecutarse en {delay.TotalSeconds:F3} segundos.");

            // Pasar el internalJobId (no el original)
            await _queue.QueueBackgroundWorkItemAsync(async sp =>
            {
                await ExecuteScheduledEmailAsync(sendEmailFunc, delay, internalJobId, cts.Token);
            });

        }

        public async Task CancelJobAsync(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                _logger.LogWarning("⚠️ Se intentó cancelar un job con ID nulo o vacío.");
                return;
            }

            // 🔥 CAMBIO 5: Buscar todos los jobs que coincidan con el patrón
            var jobsToCancel = _scheduledJobs.Keys
                .Where(k => k.StartsWith(jobId))
                .ToList();

            if (jobsToCancel.Count == 0)
            {
                _logger.LogInformation($"ℹ️ Job '{jobId}' no encontrado. Puede que ya se haya ejecutado.");
                await Task.CompletedTask;
                return;
            }

            foreach (var job in jobsToCancel)
            {
                if (_scheduledJobs.TryRemove(job, out var cts))
                {
                    try
                    {
                        cts.Cancel();
                        _logger.LogInformation($"✅ Job '{job}' cancelado exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"❌ Error al cancelar el job '{job}'.");
                    }
                    finally
                    {
                        cts.Dispose();
                    }
                }
            }

            await Task.CompletedTask;
        }

        private async Task ExecuteScheduledEmailAsync(
            Func<Task> sendEmailFunc,
            TimeSpan delay,
            string jobId,
            CancellationToken token)
        {
            try
            {
                // 🔥 CAMBIO 6: Log antes del delay
                _logger.LogDebug($"⏳ Job '{jobId}' esperando {delay.TotalSeconds:F3}s antes de ejecutar...");

                await Task.Delay(delay, token);

                token.ThrowIfCancellationRequested();

                _logger.LogInformation($"🚀 Ejecutando job '{jobId}'...");

                await sendEmailFunc();

                _logger.LogInformation($"✅ Job '{jobId}' ejecutado exitosamente.");
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation($"🗑️ Job '{jobId}' fue cancelado antes de ejecutarse.");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"🗑️ Job '{jobId}' fue cancelado (OperationCanceledException).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error ejecutando job '{jobId}'.");
            }
            finally
            {
                // 🔥 CAMBIO 7: Intentar remover con timeout
                var removed = false;
                var retries = 0;

                while (!removed && retries < 3)
                {
                    if (_scheduledJobs.TryRemove(jobId, out var cts))
                    {
                        cts?.Dispose();
                        removed = true;
                        _logger.LogDebug($"🧹 Recursos del job '{jobId}' liberados.");
                    }
                    else
                    {
                        retries++;
                        if (retries < 3)
                        {
                            await Task.Delay(50); // Esperar 50ms antes de reintentar
                        }
                    }
                }

                if (!removed)
                {
                    _logger.LogWarning($"⚠️ No se pudo remover el job '{jobId}' del diccionario después de 3 intentos.");
                }
            }
        }
    }
}