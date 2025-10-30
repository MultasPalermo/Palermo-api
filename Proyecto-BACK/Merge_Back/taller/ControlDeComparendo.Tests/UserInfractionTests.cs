using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class UserInfractionTests
    {
        [Fact]
        public void Can_Create_UserInfraction_With_Relationships()
        {
            var infraction = new UserInfraction
            {
                id = 1,
                dateInfraction = DateTime.UtcNow,
                stateInfraction = EstadoMulta.Pendiente, // 👈 enum en vez de bool
                //observations = "Exceso de velocidad",
                UserId = 10,
                InfractionId = 5,
                UserNotificationId = 3,
                UserNotification = new UserNotification { id = 3, message = "Aviso enviado" },
                Infraction = new Infraction { id = 5, description = "Velocidad" }
            };

            Assert.Equal(1, infraction.id);
            Assert.Equal(EstadoMulta.Pendiente, infraction.stateInfraction); // 👈 comparación enum
            //Assert.Equal("Exceso de velocidad", infraction.observations);
            Assert.NotNull(infraction.UserNotification);
            Assert.NotNull(infraction.Infraction);
            Assert.NotNull(infraction.paymentAgreement);
            Assert.Empty(infraction.paymentAgreement);
        }
    }
}
