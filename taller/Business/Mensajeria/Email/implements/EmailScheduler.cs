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
            await _queue.QueueBackgroundWorkItemAsync(async () =>
            {
                await Task.Delay(delay);
                await sendEmailFunc();
            });
        }
    }
}
