using Business.Interfaces.PDF;
using Business.Mensajeria.Email.@interface;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    /// <summary>
    /// servicio que programa y envía recordatorios automáticos (3, 15 y 25 días)
    /// con sus respectivos pdfs y plantillas diferentes.
    /// </summary>
    public class ReminderEmailAppService
    {
        private readonly IServiceEmail _emailService;
        private readonly IPdfGeneratorService _pdfService;
        private readonly EmailScheduler _scheduler;
        private readonly ILogger<ReminderEmailAppService> _logger;

        public ReminderEmailAppService(
            IServiceEmail emailService,
            IPdfGeneratorService pdfService,
            EmailScheduler scheduler,
            ILogger<ReminderEmailAppService> logger)
        {
            _emailService = emailService;
            _pdfService = pdfService;
            _scheduler = scheduler;
            _logger = logger;
        }

        /// <summary>
        /// programa los recordatorios reales basados en la fecha de infracción.
        /// </summary>
        public async Task ProgramarRecordatoriosAsync(UserInfractionSelectDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("❌ dto nulo. no se pueden programar recordatorios.");
                return;
            }

            if (string.IsNullOrWhiteSpace(dto.userEmail))
            {
                _logger.LogWarning($"⚠️ la infracción #{dto.id} no tiene correo asignado.");
                return;
            }

            if (dto.dateInfraction == default)
            {
                _logger.LogWarning($"⚠️ la infracción #{dto.id} no tiene fecha válida.");
                return;
            }

            _logger.LogInformation($"📅 programando recordatorios reales para infracción #{dto.id} ({dto.userEmail})");

            try
            {
                var fechaBase = DateTime.Now;

                var fechasEnvio = new[]
                {
                    (dias: 3, fecha: fechaBase.AddSeconds(5), etiqueta: "3 días"),
                    (dias: 15, fecha: fechaBase.AddSeconds(10), etiqueta: "15 días"),
                    (dias: 25, fecha: fechaBase.AddSeconds(15), etiqueta: "25 días")
                };


                foreach (var (dias, fechaEnvio, etiqueta) in fechasEnvio)
                {
                    var ahora = DateTime.Now;
                    var delay = fechaEnvio - ahora;

                    if (delay <= TimeSpan.Zero)
                    {
                        _logger.LogWarning($"⚠️ ya pasó la fecha del recordatorio de {etiqueta} para #{dto.id}. no se programará.");
                        continue;
                    }

                    await ProgramarEnvioAsync(dto, dias, etiqueta, delay);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ error al programar recordatorios para infracción #{dto.id}");
            }
        }

        private async Task ProgramarEnvioAsync(UserInfractionSelectDto dto, int dias, string etiqueta, TimeSpan delay)
        {
            await _scheduler.ScheduleEmailAsync(async () =>
            {
                try
                {
                    _logger.LogInformation($"🚀 enviando recordatorio de {etiqueta} a {dto.userEmail}...");

                    // el servicio pdf ya decidirá cuál plantilla usar según los días transcurridos
                    byte[] pdfBytes = await _pdfService.GenerateReminderPdfAsync(dto, dias);

                    if (pdfBytes == null || pdfBytes.Length == 0)
                    {
                        _logger.LogWarning($"⚠️ no se generó pdf para recordatorio de {etiqueta} (#{dto.id})");
                        return;
                    }

                    var (subject, body) = ObtenerContenidoCorreo(dto, dias);

                    using var stream = new MemoryStream(pdfBytes);
                    var attachments = new List<Attachment>
                    {
                        new Attachment(stream, $"Recordatorio_{dias}dias_{dto.id}.pdf", "application/pdf")
                    };

                    await _emailService.SendEmailAsync(dto.userEmail, subject, body, attachments);
                    _logger.LogInformation($"✅ recordatorio de {etiqueta} enviado correctamente a {dto.userEmail}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"❌ error al enviar recordatorio de {etiqueta} a {dto.userEmail}");
                }
            }, delay);
        }

        private (string subject, string body) ObtenerContenidoCorreo(UserInfractionSelectDto dto, int dias)
        {
            string subject;
            string body;

            switch (dias)
            {
                case 3:
                    subject = $"primer recordatorio - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>este es un recordatorio de pago (3 días después de la infracción).</p>";
                    break;

                case 15:
                    subject = $"segundo recordatorio - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>han pasado 15 días desde la infracción. le recordamos el pago pendiente.</p>";
                    break;

                case 25:
                    subject = $"último aviso - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>han pasado 25 días sin pago. si no cancela pronto, se iniciará cobro coactivo.</p>";
                    break;

                default:
                    subject = $"recordatorio de pago - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>le recordamos que tiene pendiente el pago de su multa.</p>";
                    break;
            }

            body += $"<p>fecha de la infracción: <b>{dto.dateInfraction:dd 'de' MMMM 'de' yyyy}</b></p>" +
                    $"<p>monto a pagar: <b>${dto.amountToPay:N0}</b></p>" +
                    "<p>atentamente,<br/>secretaría de tránsito municipal</p>";

            return (subject, body);
        }
    }
}
