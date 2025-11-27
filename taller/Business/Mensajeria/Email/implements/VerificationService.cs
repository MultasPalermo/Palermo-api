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
        private readonly IServiceEmail _emailSender;

        public VerificationService(
            EmailBackgroundQueue emailQueue,
            IServiceProvider scopeFactory,
            VerificationCache cache,    
            IServiceEmail emailSender)
        {
            _emailQueue = emailQueue;
            _scopeFactory = scopeFactory;
            _cache = cache;
            _emailSender = emailSender;
        }

        public async Task SendVerificationAsync(string email)
        {
            var code = CodeGenerator.GenerateNumericCode();

            _cache.SaveCode($"verification:{email}", code);

            await _emailQueue.QueueBackgroundWorkItemAsync(async sp =>
            {
                using var scope = sp.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IVerificationService>();

                var builder = new VerificacionEmailBuilder(code);
                await emailService.SendEmailAsync(email, builder);
            });
        }



        // 👉 este método lo pide el IVerificationService
        public async Task SendEmailAsync(string email, VerificacionEmailBuilder builder)
        {
            // Aquí usas tu servicio de envío real
            // Ejemplo:
            var emailSender = _scopeFactory.GetRequiredService<IServiceEmail>();
            await emailSender.SendEmailAsync(email, builder.GetSubject(), builder.GetBody());
        }

        public async Task SendVerificationPasswordAsync(string email)
        {
            var code = CodeGenerator.GenerateNumericCode();

            _cache.SaveCode($"passwordReset:{email}", code);

            await _emailQueue.QueueBackgroundWorkItemAsync(async sp =>
            {
                using var scope = sp.CreateScope();
                var emailSender = scope.ServiceProvider.GetRequiredService<IServiceEmail>();

                var builder = new PasswordResetEmailBuilder(code);

                await emailSender.SendEmailAsync(
                    email,
                    builder.GetSubject(),
                    builder.GetBody()
                );
            });
        }


        public bool ValidateCode(string email, string code, string type)
        {
            return _cache.ValidateCode($"{type}:{email}", code);
        }


        public async Task SendEmailPasswordAsync(string email, PasswordResetEmailBuilder builder)
        {
            await _emailSender.SendEmailAsync(
                email,
                builder.GetSubject(),
                builder.GetBody()
            );
        }

    }
}