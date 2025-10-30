using Business.Mensajeria.Email.@interface;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

//public class EmailService : IServiceEmail
//{
//    private readonly IConfiguration _config;

//    public EmailService(IConfiguration config)
//    {
//        _config = config;
//    }

//    public async Task SendEmailAsync(string to, string subject, string body, IEnumerable<Attachment>? attachments = null)
//    {
//        var smtp = new SmtpClient(_config["Email:SmtpServer"])
//        {
//            Port = int.Parse(_config["Email:Port"]),
//            Credentials = new NetworkCredential(
//                _config["Email:Username"],
//                _config["Email:Password"]
//            ),
//            EnableSsl = true
//        };

//        var mail = new MailMessage
//        {
//            From = new MailAddress(_config["Email:From"]),
//            Subject = subject,
//            Body = body,
//            IsBodyHtml = true
//        };
//        mail.To.Add(to);

//        // 👉 Agregar adjuntos
//        if (attachments != null)
//        {
//            foreach (var att in attachments)
//                mail.Attachments.Add(att);
//        }

//        await smtp.SendMailAsync(mail);
//    }

//}
