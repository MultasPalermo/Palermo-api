using Xunit;
using Entity.Domain.Models.Implements.Entities;
using System.Linq;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace ControlDeComparendo.Tests
{
    public class RolTests
    {
        [Fact]
        public void Can_Create_Rol_With_Collections_Initialized()
        {
            // Arrange
            var rol = new Rol { name = "Admin", description = "Administrador" };

            // Assert
            Assert.Equal("Admin", rol.name);
            Assert.Equal("Administrador", rol.description);
            Assert.NotNull(rol.rolUsers);
            Assert.Empty(rol.rolUsers);
            Assert.NotNull(rol.rol_form_permission);
            Assert.Empty(rol.rol_form_permission);
        }

        [Fact]
        public void Rol_Should_Allow_Adding_Relations()
        {
            // Arrange
            var user = new User { email = "Ingrid@gmail.com" };
            var rolUser = new RolUser { User = user };
            var rol = new Rol();

            rol.rolUsers.Add(rolUser);

            // Assert
            Assert.Single(rol.rolUsers);
            Assert.Equal("Ingrid", rol.rolUsers.First().User.email);
        }
    }
}

