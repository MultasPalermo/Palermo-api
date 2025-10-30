using Entity.Domain.Models.Implements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class InfraccionEmailBuilder : IEmailContentBuilder
    {
        private readonly UserInfractionSelectDto _dto;
        private readonly byte[] _pdf;

        public InfraccionEmailBuilder(UserInfractionSelectDto dto, byte[] pdf)
        {
            _dto = dto;
            _pdf = pdf;
        }

        public string GetSubject() => $"Notificación de infracción #{_dto.id}";

        public string GetBody() =>
                $@"
        <div style='font-family: Segoe UI, Roboto, sans-serif; padding: 20px; color: #333;'>
            <h2 style='color: #B22222;'>Notificación de Infracción</h2>
            
            <p>Estimado/a <b>{_dto.firstName} {_dto.lastName}</b>,</p>

            <p>
                Le informamos que se ha registrado una infracción de tipo 
                <b>{_dto.typeInfractionName}</b> el día <b>{_dto.dateInfraction:dd/MM/yyyy}</b>.
            </p>

            <p>
                <b>Descripción:</b><br/>
                {_dto.observations}
            </p>

            <p>
                Adjuntamos el comparendo en formato PDF con los detalles completos del caso.
            </p>

            <br/>
            <p style='font-size: 12px; color: #666;'>
                Atentamente,<br/>
                <b>Sistema de Gestión de Multas</b>
            </p>
        </div>";

        public IEnumerable<Attachment>? GetAttachments()
        {
            yield return new Attachment(new MemoryStream(_pdf), $"Infraccion_{_dto.id}.pdf");
        }
    }
}

