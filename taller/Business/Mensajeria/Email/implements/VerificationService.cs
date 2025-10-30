using Business.Mensajeria.Email.@interface;
using Data.Services;
using Helpers.CodigoVerification;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class VerificationService : IVerificationService
    {
        private readonly EmailBackgroundQueue _emailQueue;
        private readonly IServiceProvider _scopeFactory;
        private readonly VerificationCache _cache;

        public VerificationService(
            EmailBackgroundQueue emailQueue,
            IServiceProvider scopeFactory,
            VerificationCache cache)
        {
            _emailQueue = emailQueue;
            _scopeFactory = scopeFactory;
            _cache = cache;
        }

        public async Task SendVerificationAsync(string nombre, string email)
        {
            var code = CodeGenerator.GenerateNumericCode();

            // Guardar en cache
            _cache.SaveCode(email, code);

            // Mandar correo en segundo plano
            await _emailQueue.QueueBackgroundWorkItemAsync(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IVerificationService>();

                var builder = new VerificacionEmailBuilder(nombre, code);
                await emailService.SendEmailAsync(email, builder);
            });
        }

        public bool ValidateCode(string email, string code)
        {
            return _cache.ValidateCode(email, code);
        }

        // 👉 este método lo pide el IVerificationService
        public async Task SendEmailAsync(string email, VerificacionEmailBuilder builder)
        {
            // Aquí usas tu servicio de envío real
            // Ejemplo:
            var emailSender = _scopeFactory.GetRequiredService<IServiceEmail>();
            await emailSender.SendEmailAsync(email, builder.GetSubject(), builder.GetBody());
        }
    }
}