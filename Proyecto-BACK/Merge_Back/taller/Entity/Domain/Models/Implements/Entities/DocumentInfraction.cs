using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class DocumentInfraction : BaseModel
    {
        public int inspectoraReportId { get; set; }
        public int PaymentAgreementId { get; set; }

        //relaciones
        public PaymentAgreement paymentAgreement { get; set; }
        public InspectoraReport inspectoraReport { get; set; }
    }
}
