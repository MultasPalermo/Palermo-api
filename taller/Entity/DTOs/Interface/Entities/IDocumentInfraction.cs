using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Interface.Entities
{
    public interface IDocumentInfraction : IHasId
    {
        int inspectoraReportId { get; set; }
        int PaymentAgreementId { get; set; }
    }
}
