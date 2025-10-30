using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email
{
    public interface IEmailContentBuilder
    {
        string GetSubject();
        string GetBody();
        IEnumerable<Attachment>? GetAttachments();
    }
}
