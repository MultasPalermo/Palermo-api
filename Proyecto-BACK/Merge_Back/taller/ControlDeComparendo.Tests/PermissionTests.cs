using Xunit;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace ControlDeComparendo.Tests
{
    public class PermissionTests
    {
        [Fact]
        public void Can_Create_Permission_With_Collections_Initialized()
        {
            // Arrange
            var permission = new Permission { name = "Read", description = "Can read data" };

            // Assert
            Assert.Equal("Read", permission.name);
            Assert.Equal("Can read data", permission.description);
            Assert.NotNull(permission.rol_form_permission);
            Assert.Empty(permission.rol_form_permission);
        }
    }
}

