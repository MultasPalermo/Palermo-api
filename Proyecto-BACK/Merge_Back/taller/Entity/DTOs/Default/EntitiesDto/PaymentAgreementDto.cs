using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;
using Entity.DTOs.Interface.Entities;

namespace Entity.Domain.Models.Implements.Entities
{
    public class PaymentAgreementDto : IHasId, IPaymentAgreement
    {
        public int id { get; set; }
        public string address { get; set; }
        public string neighborhood { get; set; }
        public string AgreementDescription { get; set; }
        public DateTime expeditionCedula { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal BaseAmount { get; set; }
        public bool IsPaid { get; set; }
        public int userInfractionId { get; set; }
        public int paymentFrequencyId { get; set; }
        public int typePaymentId { get; set; }
        public int? Installments { get; set; }
        public decimal? MonthlyFee { get; set; }

    }
}