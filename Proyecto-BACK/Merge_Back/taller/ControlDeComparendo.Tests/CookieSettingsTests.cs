using Xunit;
using Microsoft.AspNetCore.Http;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace ControlDeComparendo.Tests
{
    public class CookieSettingsTests
    {
        [Fact]
        public void CookieSettings_Should_Have_Default_Values()
        {
            // Arrange
            var settings = new CookieSettings();

            // Assert
            Assert.Equal("access_token", settings.accessTokenName);
            Assert.Equal("refresh_token", settings.refreshTokenName);
            Assert.Equal("XSRF-TOKEN", settings.csrfCookieName);
            Assert.Equal("/", settings.path);
            Assert.True(settings.secure);
            Assert.Equal(SameSiteMode.None, settings.sameSiteMode);
        }
    }
}

