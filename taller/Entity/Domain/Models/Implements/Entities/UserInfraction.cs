using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Enums;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace Entity.Domain.Models.Implements.Entities
{
    public class UserInfraction : BaseModel
    {
        public DateTime dateInfraction { get; set; }
        public EstadoMulta stateInfraction {  get; set; }
        public string? InformationFine { get; set; }
        public int UserId { get; set; }          // FK
        public User User { get; set; } = null!;  // Navegación
        

        public int InfractionId { get; set; }
        public int numer_smldv { get; set; }
        public Infraction Infraction { get; set; } = null!;

        public int UserNotificationId { get; set; }
        public UserNotification UserNotification { get; set; } = null!;

        public List<PaymentAgreement> paymentAgreement { get; set; } = new();
        public decimal amountToPay { get; set; }           
        public decimal? smldvValueAtCreation { get; set; }

        public bool DiscountLocked { get; set; }

        public bool IsCoactive { get; set; }
        public DateTime? CoactiveActivatedOn { get; set; }
        public DateTime? LastInterestAppliedOn { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal InitialAmount { get; set; }
        public int DaysOfDelay { get; set; }
        public decimal TotalToPay { get; set; }



        public DateTime? paymentDue3Days { get; set; }
        public DateTime? paymentDue15Days { get; set; }
        public DateTime? paymentDue25Days { get; set; }
        public DateTime? paymentDue30Days { get; set; }
        public DateTime? paymentDue40Days { get; set; }

        public EstadoCobro StatusCollection { get; set; }
    }

}
