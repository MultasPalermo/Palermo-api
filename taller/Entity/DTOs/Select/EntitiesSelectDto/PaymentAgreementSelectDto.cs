using Entity.DTOs.Default.InstallmentSchedule;
using System;

namespace Entity.DTOs.Select.Entities
{
    public class PaymentAgreementSelectDto
    {
        public int Id { get; set; }

        // Datos de la persona
        public string PersonName { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public string DocumentType { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string address { get; set; } = null!;
        public string? Neighborhood { get; set; }
        // Datos de la infracción
        public string Infringement { get; set; } = null!;
        public string TypeFine { get; set; } = null!;
        public decimal ValorSMDLV { get; set; }

        // Datos del acuerdo
        public DateTime AgreementStart { get; set; }
        public DateTime AgreementEnd { get; set; }
        public DateTime expeditionCedula { get; set; }

        // Datos de pago
        public string PaymentMethod { get; set; } = null!;
        public string FrequencyPayment { get; set; } = null!;

        //Nuevos campos financieros
        public decimal BaseAmount { get; set; }           // monto original
        public decimal AccruedInterest { get; set; }      // intereses acumulados
        public decimal OutstandingAmount { get; set; }    // monto pendiente total

        public int? Installments { get; set; }
        public decimal? MonthlyFee { get; set; }

        //Estado del acuerdo
        public bool IsPaid { get; set; }                  // si ya se pagó
        public bool IsCoactive { get; set; }              // si ya pasó a cobro coactivo
        public DateTime? CoactiveActivatedOn { get; set; }// fecha de activación coactivo
        public DateTime? LastInterestAppliedOn { get; set; } // última vez que se calculó interés

        // 🔹 Nuevo: Cronograma de cuotas
        public List<InstallmentScheduleDto> InstallmentSchedule { get; set; } = new();
    }
}
