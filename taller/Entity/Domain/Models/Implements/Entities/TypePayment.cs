using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class TypePayment : BaseModel
    {
        public string name { get; set; }
        public int paymentAgreementId { get; set; }

        //relaciones
        public ICollection<PaymentAgreement> PaymentAgreements { get; set; } = new List<PaymentAgreement>();
    }
}
