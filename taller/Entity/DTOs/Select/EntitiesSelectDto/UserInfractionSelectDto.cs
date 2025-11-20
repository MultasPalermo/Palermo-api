    using Entity.Domain.Enums;
    using Entity.Domain.Interfaces;
    using Entity.Domain.Models.Base;
    using Entity.Domain.Models.Implements.ModelSecurity;
    using Entity.DTOs.Interface.Entities;
    using Entity.DTOs.Interface.Entities;
using Entity.DTOs.Select.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Entity.Domain.Models.Implements.Entities
    {
    public class UserInfractionSelectDto : IUserInfraction
    {
        public int id { get; set; }
        public DateTime dateInfraction { get; set; }
        public EstadoMulta stateInfraction { get; set; }

        public int userId { get; set; }
        public int typeInfractionId { get; set; }
        public int UserNotificationId { get; set; }

        public int numer_smldv { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string typeInfractionName { get; set; }
        public string? documentNumber { get; set; }
        public string observations { get; set; }

        public decimal amountToPay { get; set; }
        public decimal? smldvValueAtCreation { get; set; }

        public string userEmail { get; set; }

        // Fechas límite (todas deben ser nullable porque en tu entidad son nullable)
        public DateTime? paymentDue3Days { get; set; }
        public DateTime? paymentDue15Days { get; set; }
        public DateTime? paymentDue25Days { get; set; }
        public DateTime? paymentDue30Days { get; set; }
        public DateTime? paymentDue40Days { get; set; }

        // Nuevos campos agregados a la entidad
        public bool IsCoactive { get; set; }
        public DateTime? CoactiveActivatedOn { get; set; }
        public DateTime? LastInterestAppliedOn { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal InitialAmount { get; set; }
        public int DaysOfDelay { get; set; }
        public decimal TotalToPay { get; set; }


        public string StatusCollection { get; set; }

        public List<PaymentAgreementSelectDto> PaymentAgreements { get; set; } = new();
    }

}
