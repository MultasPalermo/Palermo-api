using Xunit;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity; // ajusta el namespace

namespace ControlDeComparendo.Tests
{
    public class RefreshTokenTests
    {
        [Fact]
        public void Can_Create_RefreshToken_With_Defaults()
        {
            // Arrange
            var token = new RefreshToken
            {
                Id = 1,
                UserId = 10,
                TokenHash = "abc123",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            // Assert
            Assert.Equal(1, token.Id);
            Assert.Equal(10, token.UserId);
            Assert.Equal("abc123", token.TokenHash);
            Assert.False(token.IsRevoked); // ✅ valor por defecto
            Assert.Null(token.ReplacedByTokenHash);
        }
    }
}

