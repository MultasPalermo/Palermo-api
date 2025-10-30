using Entity.Domain.Models.Implements.Entities;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class TypePaymentTests
    {
        [Fact]
        public void Can_Create_TypePayment_With_Relationship()
        {
            var agreement = new PaymentAgreement { id = 2, address = "Calle 123" };

            var typePayment = new TypePayment
            {
                id = 1,
                name = "Pago en efectivo",
                PaymentAgreements = new List<PaymentAgreement> { agreement }
            };

            Assert.Equal(1, typePayment.id);
            Assert.Equal("Pago en efectivo", typePayment.name);
            Assert.Single(typePayment.PaymentAgreements);
        }

    }
}
