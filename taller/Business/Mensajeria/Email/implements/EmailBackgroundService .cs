using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly EmailBackgroundQueue _queue;
        private readonly IServiceProvider _serviceProvider;

        public EmailBackgroundService(EmailBackgroundQueue queue, IServiceProvider serviceProvider)
        {
            _queue = queue;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var workItem in _queue.DequeueAsync(stoppingToken))
            {
                try
                {
                    // Crea un scope de DI (bueno si dentro del workItem necesitas servicios Scoped como DbContext)
                    using var scope = _serviceProvider.CreateScope();

                    // Ejecuta la tarea
                    await workItem();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error enviando correo: {ex.Message}");
                }
            }
        }
    }

}

