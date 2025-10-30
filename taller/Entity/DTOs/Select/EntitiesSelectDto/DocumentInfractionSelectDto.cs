

using Entity.Domain.Interfaces;
using Entity.DTOs.Interface.Entities;

namespace Entity.Domain.Models.Implements.Entities
{
    public class DocumentInfractionSelectDto : IDocumentInfraction
    {
        public int id { get; set; }
        public int inspectoraReportId { get; set; }
        public int PaymentAgreementId { get; set; }
        public string inspectoraReportName { get; set; }
        public string PaymentAgreementName { get; set; }
    }
}
