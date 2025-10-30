using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.parameters;

public class PaymentAgreement : BaseModel
{
    public string address { get; set; } = null!;   // sigue siendo obligatorio
    public string? neighborhood { get; set; }      // ahora puede ser null
    public string? AgreementDescription { get; set; } // ahora puede ser null
    public DateTime? expeditionCedula { get; set; }  // ahora puede ser null
    public string? PhoneNumber { get; set; }       // ahora puede ser null
    public string? Email { get; set; }             // ahora puede ser null
    public DateTime AgreementStart { get; set; }
    public DateTime AgreementEnd { get; set; }

    public int userInfractionId { get; set; }
    public UserInfraction userInfraction { get; set; }

    public int paymentFrequencyId { get; set; }
    public PaymentFrequency paymentFrequency { get; set; }

    public int typePaymentId { get; set; }
    public TypePayment TypePayment { get; set; }

    // --- Financieros / de estado ---
    public decimal BaseAmount { get; set; }              // Monto original del acuerdo
    public decimal AccruedInterest { get; set; } = 0m;   // Intereses acumulados
    public decimal OutstandingAmount { get; set; }

    public int?Installments { get; set; }   // número de cuotas
    public decimal? MonthlyFee { get; set; } // valor de cada cuota

    public bool IsPaid { get; set; } = false;            // Marcado cuando se cancela totalmente
    public bool IsCoactive { get; set; } = false;        // Se activa al día 30 si no se ha pagado
    public DateTime? CoactiveActivatedOn { get; set; }   // Fecha de activación
    public DateTime? LastInterestAppliedOn { get; set; } // Último día para el cual ya calculaste interés (a nivel de fecha, no hora)


    public List<DocumentInfraction> documentInfraction { get; set; } = new List<DocumentInfraction>();
    public List<InstallmentSchedule> InstallmentSchedule { get; set; } = new List<InstallmentSchedule>();
}

