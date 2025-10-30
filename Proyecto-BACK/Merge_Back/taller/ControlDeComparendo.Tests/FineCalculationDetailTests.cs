using Entity.Domain.Models.Implements.Entities;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class FineCalculationDetailTests
    {
        [Fact]
        public void Can_Create_FineCalculationDetail_With_Relationships()
        {
            var detail = new FineCalculationDetail
            {
                id = 1,
                formula = "SMLDV * 2",
                totalCalculation = 300000,
                valueSmldvId = 10,
                typeInfractionId = 5,
                valueSmldv = new ValueSmldv { id = 10, minimunWage = 1300000 },
                Infraction = new Infraction
                {
                    id = 5,
                    description = "Infracción grave",
                    numer_smldv = 8 // 🔹 Nuevo campo requerido
                }
            };

            Assert.Equal(1, detail.id);
            Assert.Equal("SMLDV * 2", detail.formula);
            Assert.NotNull(detail.valueSmldv);
            Assert.NotNull(detail.Infraction);
            Assert.Equal(8, detail.Infraction.numer_smldv); 
        }
    }
}
