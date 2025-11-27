using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class EmailScheduler
    {
        private readonly EmailBackgroundQueue _queue;

        public EmailScheduler(EmailBackgroundQueue queue)
        {
            _queue = queue;
        }

        /// <summary>
        /// Agenda el envío de un correo después de cierto tiempo.
        /// </summary>
        public async Task ScheduleEmailAsync(Func<Task> sendEmailFunc, TimeSpan delay)
        {
            if (sendEmailFunc == null)
                throw new ArgumentNullException(nameof(sendEmailFunc));

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

        /// <summary>
        /// Ejecuta el envío real luego del delay.
        /// Mantiene SRP: una sola responsabilidad = ejecutar el envío.
        /// </summary>
        private async Task ExecuteScheduledEmailAsync(Func<Task> sendEmailFunc, TimeSpan delay)
        {
            try
            {
                await Task.Delay(delay);
                await sendEmailFunc();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error ejecutando tarea programada: {ex.Message}");
            }
        }

    }
}
