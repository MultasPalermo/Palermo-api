

using Entity.Domain.Interfaces;
using Entity.DTOs.Interface.Entities;

namespace Entity.Domain.Models.Implements.Entities
    {
        public class DocumentInfractionDto : IHasId, IDocumentInfraction
    {
            public int id { get; set; }
            public int inspectoraReportId { get; set; }
            public int PaymentAgreementId { get; set; }
        }
    }
