using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Select.EntitiesSelectDto
{
    public class InstallmentScheduleSelectDto
    {
        public int Id { get; set; }                         // Identificador único
        public int Number { get; set; }                     // Número de la cuota
        public DateTime PaymentDate { get; set; }           // Fecha programada de pago
        public decimal Amount { get; set; }                 // Valor de la cuota
        public decimal RemainingBalance { get; set; }       // Saldo pendiente después de esta cuota
        public bool IsPaid { get; set; }                    // Indica si la cuota fue pagada

        // 🆕 Campos derivados (opcionales pero muy útiles para mostrar en reportes o tablas)
        public string Status => IsPaid ? "Pagada" : "Pendiente";   // Texto legible del estado
        public string PaymentDateFormatted => PaymentDate.ToString("dd/MM/yyyy"); // Fecha legible
    }
}
