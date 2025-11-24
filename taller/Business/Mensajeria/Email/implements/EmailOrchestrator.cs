using Business.Interfaces.PDF;
using Business.Mensajeria.Email.@interface;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class EmailOrchestrator
    {
        private readonly IServiceEmail _emailService;
        private readonly IPdfGeneratorService _pdfService;
        private readonly ReminderEmailAppService _reminderService;
        private readonly ILogger<EmailOrchestrator> _logger;

        public EmailOrchestrator(
            IServiceEmail emailService,
            IPdfGeneratorService pdfService,
            ReminderEmailAppService reminderService,
            ILogger<EmailOrchestrator> logger)
        {
            _emailService = emailService;
            _pdfService = pdfService;
            _reminderService = reminderService;
            _logger = logger;
        }

        public async Task ProcesarNotificacionInicialAsync(UserInfractionSelectDto dto)
        {
            try
            {
                // Generar PDF de la multa
                var pdfBytes = await _pdfService.GeneratePdfAsync(dto);

                // Construir el correo con el PDF adjunto
                var builder = new InfraccionEmailBuilder(dto, pdfBytes);

                // Enviar correo inicial
                await _emailService.SendEmailAsync(
                    dto.userEmail,
                    builder.GetSubject(),
                    builder.GetBody(),
                    builder.GetAttachments()?.ToList()
                );

                _logger.LogInformation($"✅ Notificación inicial enviada a {dto.userEmail} para multa #{dto.id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error en procesamiento inicial de infracción #{dto.id}");
            }
        }
    }

}
