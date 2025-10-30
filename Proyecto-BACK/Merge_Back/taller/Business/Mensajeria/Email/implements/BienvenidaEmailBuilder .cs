using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class BienvenidaEmailBuilder : IEmailContentBuilder
    {
        public string GetSubject() => "¡Bienvenido a nuestro sistema!";

        public string GetBody() => @"
            <div style='font-family: Segoe UI, Roboto, sans-serif; padding: 40px;'>
                <h1 style='color:#434343;'>¡Bienvenido a Nuestra Plataforma!</h1>
                <p>Estamos emocionados de que formes parte de nuestra comunidad.</p>
                <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
            </div>";

        public IEnumerable<Attachment>? GetAttachments() => null;
    }
}

