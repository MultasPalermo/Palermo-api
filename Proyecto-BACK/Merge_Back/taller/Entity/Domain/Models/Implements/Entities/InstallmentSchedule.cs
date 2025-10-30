using Entity.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.Entities
{
        public class InstallmentSchedule : BaseModel
        {
            public int Number { get; set; }                 // número de cuota
            public DateTime PaymentDate { get; set; }       // fecha de pago
            public decimal Amount { get; set; }             // valor de la cuota
            public decimal RemainingBalance { get; set; }   // saldo restante después del pago
            public bool IsPaid { get; set; } = false;       // indica si la cuota fue pagada

            // 🔗 Relación con PaymentAgreement
            public int PaymentAgreementId { get; set; }
            public PaymentAgreement PaymentAgreement { get; set; }
        }
}
