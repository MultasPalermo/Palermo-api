using Xunit;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace ControlDeComparendo.Tests
{
    public class FormTests
    {
        [Fact]
        public void Can_Create_Form_With_Collections_Initialized()
        {
            // Arrange
            var form = new Form { name = "Dashboard", description = "Formulario principal" };

            // Assert
            Assert.Equal("Dashboard", form.name);
            Assert.Equal("Formulario principal", form.description);
            Assert.NotNull(form.rol_form_permission);
            Assert.Empty(form.rol_form_permission);
            Assert.NotNull(form.FormModules);
            Assert.Empty(form.FormModules);
        }
    }
}
