using Entity.Domain.Models.Implements.Entities;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class UserNotificationTests
    {
        [Fact]
        public void UserNotification_Should_Have_Default_Empty_List()
        {
            var notification = new UserNotification
            {
                id = 1,
                message = "Notificación enviada",
                shippingDate = DateTime.UtcNow
            };

            Assert.Equal(1, notification.id);
            Assert.Equal("Notificación enviada", notification.message);
            Assert.NotNull(notification.userInfraction);
            Assert.Empty(notification.userInfraction);
        }
    }
}
