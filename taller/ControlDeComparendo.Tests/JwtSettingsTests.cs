using Entity.Domain.Models.Implements.ModelSecurity;
using Xunit;

namespace ControlDeComparendo.Tests
{
    public class JwtSettingsTests
    {
        [Fact]
        public void JwtSettings_Should_Have_Default_Values()
        {
            // Arrange
            var settings = new JwtSettings();

            // Assert
            Assert.Equal("WebCDCP.API", settings.Issuer);
            Assert.Equal("WebCDCP.Client", settings.Audience);
            Assert.Equal(15, settings.AccessTokenExpirationMinutes);
            Assert.Equal(7, settings.RefreshTokenExpirationDays);
        }
    }
}

