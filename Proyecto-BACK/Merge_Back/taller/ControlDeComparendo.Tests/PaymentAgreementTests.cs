using Entity.Domain.Models.Implements.Entities;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class PaymentAgreementTests
    {
        [Fact]
        public void PaymentAgreement_Should_Have_Default_Values_And_Collections()
        {
            // Arrange
            var agreement = new PaymentAgreement
            {
                id = 1,
                address = "Calle 45",
                neighborhood = "Centro",
                AgreementDescription = "Acuerdo de pago en cuotas",
                BaseAmount = 100000m, // monto base simulado
                OutstandingAmount = 100000m // igual al monto base al inicio
            };

            // Act & Assert
            Assert.Equal(1, agreement.id);
            Assert.Equal("Centro", agreement.neighborhood);

            // Colecciones
            Assert.NotNull(agreement.documentInfraction);
            Assert.Empty(agreement.documentInfraction);

            // FK aún no asignada
            Assert.Null(agreement.TypePayment);

            // Financieros
            Assert.Equal(100000m, agreement.BaseAmount);
            Assert.Equal(0m, agreement.AccruedInterest);
            Assert.Equal(100000m, agreement.OutstandingAmount);

            // Estado inicial
            Assert.False(agreement.IsPaid);
            Assert.False(agreement.IsCoactive);
            Assert.Null(agreement.CoactiveActivatedOn);
            Assert.Null(agreement.LastInterestAppliedOn);
        }
    }
}
