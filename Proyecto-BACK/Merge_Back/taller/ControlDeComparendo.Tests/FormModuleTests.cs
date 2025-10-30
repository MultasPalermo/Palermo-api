using Xunit;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace ControlDeComparendo.Tests
{
    public class FormModuleTests
    {
        [Fact]
        public void Can_Create_FormModule_With_ForeignKeys()
        {
            // Arrange
            var form = new Form { name = "Dashboard" };
            var module = new Module { name = "Seguridad" };

            var formModule = new FormModule
            {
                formid = 1,
                moduleid = 2,
                form = form,
                module = module
            };

            // Assert
            Assert.Equal(1, formModule.formid);
            Assert.Equal(2, formModule.moduleid);
            Assert.Equal("Dashboard", formModule.form.name);
            Assert.Equal("Seguridad", formModule.module.name);
        }
    }
}
