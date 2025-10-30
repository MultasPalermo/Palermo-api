using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.InstallmentSchedule
{
    public class InstallmentScheduleDto
    {
        public int Number { get; set; }            // Número de cuota
        public DateTime PaymentDate { get; set; }  // Fecha de pago
        public decimal Amount { get; set; }        // Valor de la cuota
        public decimal RemainingBalance { get; set; } // Saldo restante después del pago
    }

}
