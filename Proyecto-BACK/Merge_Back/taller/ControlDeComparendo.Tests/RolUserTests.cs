using Xunit;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace ControlDeComparendo.Tests
{
    public class RolUserTests
    {
        [Fact]
        public void Can_Create_RolUser_With_ForeignKeys()
        {
            // Arrange
            var rolUser = new RolUser
            {
                UserId = 1,
                RolId = 2
            };

            // Assert
            Assert.Equal(1, rolUser.UserId);
            Assert.Equal(2, rolUser.RolId);
        }

        [Fact]
        public void RolUser_Should_Allow_Navigation()
        {
            // Arrange
            var user = new User { email = "Ingrid@gmail.com" };
            var rol = new Rol { name = "Admin" };

            var rolUser = new RolUser
            {
                User = user,
                Rol = rol
            };

            // Assert
            Assert.NotNull(rolUser.User);
            Assert.NotNull(rolUser.Rol);
            Assert.Equal("Ingrid", rolUser.User.email);
            Assert.Equal("Admin", rolUser.Rol.name);
        }
    }
}

