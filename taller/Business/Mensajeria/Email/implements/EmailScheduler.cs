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

            // Solo se encarga de AGENDAR
            await _queue.QueueBackgroundWorkItemAsync(() =>
                ExecuteScheduledEmailAsync(sendEmailFunc, delay)
            );
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
