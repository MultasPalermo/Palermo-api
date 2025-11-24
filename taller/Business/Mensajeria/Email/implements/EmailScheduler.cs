using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    // ✅ Implementar la interfaz y usar ConcurrentDictionary
    public class EmailScheduler : IReminderScheduler
    {
        private readonly EmailBackgroundQueue _queue;
        // 💡 Almacena la fuente de cancelación (CTS) por el ID del trabajo (JobId)
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _scheduledJobs;

        public EmailScheduler(EmailBackgroundQueue queue)
        {
            _queue = queue;
            _scheduledJobs = new ConcurrentDictionary<string, CancellationTokenSource>();
        }

        // ----------------------------------------
        // IMPLEMENTACIÓN DE IREMINDERSCHEDULER
        // ----------------------------------------

        /// <summary>
        /// Agenda el envío de un correo después de cierto tiempo.
        /// </summary>
        // ✅ 1. Recibe el jobId (ej: "Infraction_6_Status_prejuridico25Dias")
        public async Task ScheduleEmailAsync(Func<Task> sendEmailFunc, TimeSpan delay, string jobId)
        {
            if (sendEmailFunc == null)
                throw new ArgumentNullException(nameof(sendEmailFunc));
            if (string.IsNullOrWhiteSpace(jobId))
                throw new ArgumentNullException(nameof(jobId));

            // Crear y almacenar la fuente de cancelación para esta tarea
            var cts = new CancellationTokenSource();
            if (!_scheduledJobs.TryAdd(jobId, cts))
            {
                // Si ya existe (no debería ocurrir si se usa correctamente), retornar
                return;
            }

            // Agendar la ejecución con los nuevos parámetros
            await _queue.QueueBackgroundWorkItemAsync(() =>
                ExecuteScheduledEmailAsync(sendEmailFunc, delay, jobId, cts.Token)
            );
        }

        /// <summary>
        /// Cancela una tarea programada por su ID.
        /// </summary>
        // ✅ 2. Detiene la ejecución pendiente.
        public async Task CancelJobAsync(string jobId)
        {
            // Intentar remover la fuente de cancelación del diccionario
            if (_scheduledJobs.TryRemove(jobId, out var cts))
            {
                // Disparar la cancelación. Esto lanzará TaskCanceledException dentro de ExecuteScheduledEmailAsync.
                cts.Cancel();
                cts.Dispose(); // Liberar recursos
            }
            await Task.CompletedTask;
        }

        // ----------------------------------------
        // MÉTODOS PRIVADOS (AUXILIARES)
        // ----------------------------------------

        /// <summary>
        /// Ejecuta el envío real luego del delay.
        /// </summary>
        // ✅ 3. Recibe el jobId y el CancellationToken asociado
        private async Task ExecuteScheduledEmailAsync(
            Func<Task> sendEmailFunc,
            TimeSpan delay,
            string jobId,
            CancellationToken token)
        {
            try
            {
                // 🛑 4. CRÍTICO: Monitorear el token durante el retraso. Si se cancela, lanza TaskCanceledException.
                await Task.Delay(delay, token);

                // ✅ 5. Monitorear el token durante la ejecución real de la función
                // (Aunque Task.Delay es lo más importante aquí, es buena práctica pasarlo a la función)
                token.ThrowIfCancellationRequested();

                await sendEmailFunc();
            }
            catch (TaskCanceledException)
            {
                // ✅ Manejar la cancelación (para evitar el log de error)
                Console.WriteLine($"🗑️ Tarea programada '{jobId}' cancelada correctamente. No se enviará correo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error ejecutando tarea programada '{jobId}': {ex.Message}");
            }
            finally
            {
                // 6. Limpiar: Remover la entrada del diccionario, si aún estuviera allí (TryRemove es más seguro)
                _scheduledJobs.TryRemove(jobId, out _);
            }
        }
    }
}