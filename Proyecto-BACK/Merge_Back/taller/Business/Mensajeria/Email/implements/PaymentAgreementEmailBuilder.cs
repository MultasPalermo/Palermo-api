using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Entity.DTOs.Select.Entities;

namespace Business.Mensajeria.Email.implements
{
    internal class PaymentAgreementEmailBuilder
    {
        private readonly PaymentAgreementSelectDto _dto;
        private readonly byte[] _pdfBytes;

        public PaymentAgreementEmailBuilder(PaymentAgreementSelectDto dto, byte[] pdfBytes)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
            _pdfBytes = pdfBytes ?? throw new ArgumentNullException(nameof(pdfBytes));
        }

        public string GetSubject()
        {
            return $"Acuerdo de Pago - {_dto.PersonName}";
        }

        public string GetBody()
        {
            return $@"
<p>Hola {_dto.PersonName},</p>
<p>Se ha generado tu acuerdo de pago correspondiente a la infracción {_dto.Infringement ?? "-"}.</p>
<p>Adjunto encontrarás el PDF con todos los detalles del acuerdo.</p>
<p>Gracias por tu atención.</p>
<p>Sistema de Gestión de Multas</p>
";
        }

        public IEnumerable<Attachment> GetAttachments()
        {
            // Creamos un MemoryStream a partir del PDF
            var stream = new MemoryStream(_pdfBytes);
            var attachment = new Attachment(stream, $"AcuerdoPago_{_dto.PersonName}_{DateTime.Now:yyyyMMdd}.pdf", "application/pdf");

            return new List<Attachment> { attachment };
        }
    }
}
