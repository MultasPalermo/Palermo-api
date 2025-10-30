using Xunit;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace ControlDeComparendo.Tests
{
    public class RolFormPermissionTests
    {
        [Fact]
        public void Can_Create_RolFormPermission_With_ForeignKeys()
        {
            // Arrange
            var permission = new RolFormPermission
            {
                RolId = 1,
                FormId = 2,
                PermissionId = 3
            };

            // Assert
            Assert.Equal(1, permission.RolId);
            Assert.Equal(2, permission.FormId);
            Assert.Equal(3, permission.PermissionId);
        }

        [Fact]
        public void RolFormPermission_Should_Allow_Navigation()
        {
            // Arrange
            var rol = new Rol { name = "Admin" };
            var form = new Form { name = "Dashboard" };
            var perm = new Permission { name = "Read" };

            var rfp = new RolFormPermission
            {
                Rol = rol,
                Form = form,
                Permission = perm
            };

            // Assert
            Assert.Equal("Admin", rfp.Rol.name);
            Assert.Equal("Dashboard", rfp.Form.name);
            Assert.Equal("Read", rfp.Permission.name);
        }
    }
}

