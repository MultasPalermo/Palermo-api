using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Init
{
    public class PaymentAgreementInitDto
    {
        // Datos de la persona
        public string PersonName { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public string DocumentType { get; set; } = null!;

        // Datos de la infracción
        public int InfractionId { get; set; }
        public int UserId { get; set; }
        public string Infringement { get; set; } = null!;
        public decimal BaseAmount { get; set;  }
        public string TypeFine { get; set; } = null!;
        public decimal ValorSMDLV { get; set; }
    }
}
