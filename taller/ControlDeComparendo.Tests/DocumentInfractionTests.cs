using Entity.Domain.Models.Implements.Entities;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class DocumentInfractionTests
    {
        [Fact]
        public void Can_Create_DocumentInfraction_With_Relationships()
        {
            var doc = new DocumentInfraction
            {
                id = 1,
                inspectoraReportId = 3,
                PaymentAgreementId = 7,
                paymentAgreement = new PaymentAgreement { id = 7, address = "Av 80" },
                inspectoraReport = new InspectoraReport { id = 3, message = "Reporte mensual" }
            };

            Assert.Equal(1, doc.id);
            Assert.Equal(3, doc.inspectoraReportId);
            Assert.Equal(7, doc.PaymentAgreementId);
            Assert.NotNull(doc.paymentAgreement);
            Assert.NotNull(doc.inspectoraReport);
        }
    }
}

