using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.@interface
{
    public interface IServiceEmail
    {
        //Task EnviarHtmlAsync(string to, string subject, string htmlBody);

        Task SendEmailAsync(string to, string subject, string body, IEnumerable<Attachment>? attachments = null);

        Task SendEmailAsyncVerificacion(string to, IEmailContentBuilder builder);

       // Task SendEmailAsync(string to, IEmailContentBuilder builder);


    }
}
