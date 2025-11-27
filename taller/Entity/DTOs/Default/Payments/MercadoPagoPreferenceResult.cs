using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Payments
{
    public class MercadoPagoPreferenceResult
    {
        public required string InitPoint { get; set; }
        public required string PreferenceId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "COP";
        public int ObligationId { get; set; }
        public int ContractId { get; set; }
        public string? PaymentId { get; set; }
    }
}
