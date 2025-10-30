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
    public class TypePaymentDto : IHasId, ITypePayment
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int paymentAgreementId { get; set; }
    }
}
