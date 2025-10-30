using Xunit;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity; // Ajusta el namespace de tu entidad User

namespace ControlDeComparendo.Tests
{
    public class UserTests
    {
        [Fact]
        public void Can_Create_User_With_Properties()
        {
            // Arrange
            var user = new User
            {
           
                email = "ingrid@test.com",
                PasswordHash = "12345",
                PersonId = 1
            };

            // Act
            var resultEmail = user.email;

            // Assert
            Assert.Equal("Ingrid", resultEmail);
            Assert.Equal("ingrid@test.com", resultEmail);
            Assert.Equal(1, user.PersonId);
        }

        [Fact]
        public void User_Should_Initialize_Empty_Collections()
        {
            // Arrange
            var user = new User();

            // Assert
            Assert.NotNull(user.UserInfraction);
            Assert.NotNull(user.rolUsers);
            Assert.Empty(user.UserInfraction);
            Assert.Empty(user.rolUsers);
        }
    }
}

