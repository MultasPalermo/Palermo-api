using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class PasswordResetEmailBuilder : IEmailContentBuilder
    {
        private readonly string _nombre;
        private readonly string _codigo;

        public PasswordResetEmailBuilder(string codigo)
        {
            _codigo = codigo;
        }

        public string GetSubject() => "🔐 código para restablecer tu contraseña";

        public string GetBody() =>
            $@"
<!DOCTYPE html>
<html lang='es'>
<body style='font-family: Inter, sans-serif; padding: 20px; background: #f4f6f9;'>

    <h2 style='color:#1a202c; text-align:center;'>restablecer contraseña</h2>

    <p style='text-align:center; font-size:16px; color:#4a5568;'>
        hola <strong></strong>, usa el siguiente código para restablecer tu contraseña:
    </p>

    <div style='margin: 40px auto; width: fit-content; padding: 25px 40px;
                border: 2px dashed #3182ce; border-radius: 12px;
                font-size: 38px; font-weight: bold; color: #3182ce;
                letter-spacing: 10px;'>
        {_codigo}
    </div>

    <p style='text-align:center; font-size:14px; color:#718096;'>
        este código expira en <strong>10 minutos</strong>.
    </p>

</body>
</html>";

        public IEnumerable<Attachment>? GetAttachments() => null;
    }
}
