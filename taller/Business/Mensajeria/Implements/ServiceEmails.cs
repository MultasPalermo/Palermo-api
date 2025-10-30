using Business.Mensajeria.Email;
using Business.Mensajeria.Email.@interface;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Business.Mensajeria.Implements
{
    public class ServiceEmails : IServiceEmail
    {
        private readonly IConfiguration _config;

        public ServiceEmails(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body, IEnumerable<Attachment>? attachments = null)
        {
            var host = _config["SmtpSettings:Host"];
            var port = _config.GetValue<int>("SmtpSettings:Port");
            var from = _config["SmtpSettings:Email"];
            var pass = _config["SmtpSettings:Password"];


            using var smtp = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, pass)
            };

            using var msg = new MailMessage
            {
                From = new MailAddress(from, "Proyecto Hacienda"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };
            msg.To.Add(to);

            if (attachments != null)
            {
                foreach (var att in attachments)
                    msg.Attachments.Add(att);
            }

            await smtp.SendMailAsync(msg);
        }
        // 🔹 Implementación de la interfaz con builder
        public async Task SendEmailAsync(string to, IEmailContentBuilder builder)
        {
            await SendEmailAsync(to, builder.GetSubject(), builder.GetBody(), builder.GetAttachments());
        }

        public async Task SendEmailAsyncVerificacion(string to, IEmailContentBuilder builder)
        {
            await SendEmailAsync(to, builder.GetSubject(), builder.GetBody(), builder.GetAttachments());
        }
    }
}
