using Xunit;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace ControlDeComparendo.Tests
{
    public class ModuleTests
    {
        [Fact]
        public void Can_Create_Module_With_Collections_Initialized()
        {
            // Arrange
            var module = new Module { name = "Seguridad", description = "Módulo de seguridad" };

            // Assert
            Assert.Equal("Seguridad", module.name);
            Assert.Equal("Módulo de seguridad", module.description);
            Assert.NotNull(module.FormModules);
            Assert.Empty(module.FormModules);
        }
    }
}
