using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class VerificacionEmailBuilder : IEmailContentBuilder
    {
        private readonly string _nombre;
        private readonly string _codigo;

        public VerificacionEmailBuilder(string nombre, string codigo)
        {
            _nombre = nombre;
            _codigo = codigo;
        }

        public string GetSubject() => "Código de verificación";

        public string GetBody() =>
            $@"
    <div style='font-family: Arial, sans-serif; color: #333; padding: 20px;'>
        <h2 style='color: #4CAF50;'>👋 Hola {_nombre},</h2>
        <p style='font-size: 16px;'>Gracias por usar nuestro sistema.</p>
        
        <p style='font-size: 16px;'>
            Tu <b>código de verificación</b> es:
        </p>
        
        <div style='
            background-color: #f4f4f4;
            border: 2px dashed #4CAF50;
            color: #2c3e50;
            font-size: 24px;
            font-weight: bold;
            text-align: center;
            padding: 15px;
            margin: 20px 0;
            border-radius: 8px;'>
            {_codigo}
        </div>

        <p style='font-size: 14px; color: #777;'>
            ⏳ Este código expira en <b>15 minutos</b>.
        </p>

        <p style='font-size: 14px; color: #999;'>
            Si no solicitaste este código, por favor ignora este mensaje.
        </p>
    </div>";


        public IEnumerable<Attachment>? GetAttachments()
        {
            return null; // No hay adjuntos
        }
    }
}
