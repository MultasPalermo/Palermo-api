using System;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email
{
    public interface IReminderScheduler
    {
        // ✅ 1. Método para cancelar una tarea por su ID.
        Task CancelJobAsync(string jobId);

        // ✅ 2. Método para programar una tarea, ahora requiere el jobId.
        Task ScheduleEmailAsync(Func<Task> sendEmailFunc, TimeSpan delay, string jobId);
    }
}