using Xunit;
using Moq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Business.Services.Security;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Business.Interfaces.IBusinessImplements.Security;
using Data.Interfaces.IDataImplement.Security;
using Business.Mensajeria.Email.@interface;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.validaciones.Entities.UserInfraction;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.AnexarMulta;
using Entity.DTOs.Default.RegisterRequestDto;
using Entity.Infrastructure.Contexts;
using Utilities.Custom;
using Web.AutoMapper;

namespace ControlDeComparendo.Tests.Services
{
    public class UserServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPersonRepository> _personRepoMock;
        private readonly Mock<IRolUserService> _rolUserMock;
        private readonly Mock<IServiceEmail> _emailMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly EncriptePassword _passwordUtil;
        private readonly ApplicationDbContext _db;

        public UserServiceTests()
        {
            // AutoMapper config
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = mapperConfig.CreateMapper();

            // Mocks
            _userRepoMock = new Mock<IUserRepository>();
            _personRepoMock = new Mock<IPersonRepository>();
            _rolUserMock = new Mock<IRolUserService>();
            _emailMock = new Mock<IServiceEmail>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _passwordUtil = new EncriptePassword();

            // =====================================================
            //   CONFIGURAR SQLITE IN MEMORY (ACEPTA TRANSACCIONES)
            // =====================================================
            var connection = new Microsoft.Data.Sqlite.SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            _db = new ApplicationDbContext(options);
            _db.Database.EnsureCreated(); // NECESARIO PARA SQLite InMemory
        }


        // -------------------------------------------------------------
        // 1) TEST: Registro exitoso
        // -------------------------------------------------------------
        [Fact]
        public async Task RegisterAsync_Should_Register_User_Successfully()
        {
            // Arrange
            var dto = new RegisterRequestDto
            {
                NombreCompleto = "John Doe",
                email = "test@mail.com",
                password = "123456"
            };

            _userRepoMock
                .Setup(r => r.FindEmail(dto.email))
                .ReturnsAsync((User)null);

            _personRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<Person>()))
                .ReturnsAsync(new Person { id = 10, firstName = dto.NombreCompleto });

            _userRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(new User { id = 99, email = dto.email });

            var service = new UserService(
                _userRepoMock.Object,
                _personRepoMock.Object,
                _db,
                _loggerMock.Object,
                _passwordUtil,
                _mapper,
                _rolUserMock.Object,
                _emailMock.Object
            );

            // Act
            var result = await service.RegisterAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains("Registro completado", result.Message);

            _personRepoMock.Verify(r => r.CreateAsync(It.IsAny<Person>()), Times.Once);
            _userRepoMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
        }

        // -------------------------------------------------------------
        // 2) TEST: Email ya existe
        // -------------------------------------------------------------
        [Fact]
        public async Task RegisterAsync_Should_Throw_When_Email_Already_Exists()
        {
            // Arrange
            var dto = new RegisterRequestDto { email = "existing@mail.com" };

            _userRepoMock
                .Setup(r => r.FindEmail(dto.email))
                .ReturnsAsync(new User { email = dto.email, is_deleted = false });

            var service = new UserService(
                _userRepoMock.Object,
                _personRepoMock.Object,
                _db,
                _loggerMock.Object,
                _passwordUtil,
                _mapper,
                _rolUserMock.Object,
                _emailMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterAsync(dto));
        }

        // -------------------------------------------------------------
        // 3) TEST: Verificar código exitoso
        // -------------------------------------------------------------
        [Fact]
        public async Task VerifyCodeAsync_Should_Verify_Correct_Code()
        {
            // Arrange
            var user = new User
            {
                EmailVerificationCode = "123456",
                EmailVerificationExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            _userRepoMock
                .Setup(r => r.FindByVerificationCodeAsync("123456", default))
                .ReturnsAsync(user);

            var service = new UserService(
                _userRepoMock.Object,
                _personRepoMock.Object,
                _db,
                _loggerMock.Object,
                _passwordUtil,
                _mapper,
                _rolUserMock.Object,
                _emailMock.Object
            );

            // Act
            var result = await service.VerifyCodeAsync("123456");

            // Assert
            Assert.True(result);
            Assert.True(user.EmailVerified);
        }

        // -------------------------------------------------------------
        // 4) TEST: Código expirado
        // -------------------------------------------------------------
        [Fact]
        public async Task VerifyCodeAsync_Should_Fail_When_Code_Expired()
        {
            var user = new User
            {
                EmailVerificationCode = "999999",
                EmailVerificationExpiresAt = DateTime.UtcNow.AddMinutes(-10)
            };

            _userRepoMock
                .Setup(r => r.FindByVerificationCodeAsync("999999", default))
                .ReturnsAsync(user);

            var service = new UserService(
                _userRepoMock.Object,
                _personRepoMock.Object,
                _db,
                _loggerMock.Object,
                _passwordUtil,
                _mapper,
                _rolUserMock.Object,
                _emailMock.Object
            );

            var result = await service.VerifyCodeAsync("999999");

            Assert.False(result);
        }

        // -------------------------------------------------------------
        // 5) TEST: createUserGoogle crea uno nuevo
        // -------------------------------------------------------------
        [Fact]
        public async Task CreateUserGoogle_Should_Create_User_If_Not_Exists()
        {
            _userRepoMock
                .Setup(r => r.FindEmail("google@mail.com"))
                .ReturnsAsync((User)null);

            _userRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(new User { email = "google@mail.com" });

            var service = new UserService(
                _userRepoMock.Object,
                _personRepoMock.Object,
                _db,
                _loggerMock.Object,
                _passwordUtil,
                _mapper,
                _rolUserMock.Object,
                _emailMock.Object
            );

            var result = await service.createUserGoogle("google@mail.com", "Google User");

            Assert.Equal("google@mail.com", result.email);
        }

        // -------------------------------------------------------------
        // 6) TEST: UpdateAsyncUser lanza error si DTO es null
        // -------------------------------------------------------------
        [Fact]
        public async Task UpdateAsyncUser_Should_Throw_When_DTO_Is_Null()
        {
            var service = new UserService(
                _userRepoMock.Object,
                _personRepoMock.Object,
                _db,
                _loggerMock.Object,
                _passwordUtil,
                _mapper,
                _rolUserMock.Object,
                _emailMock.Object
            );

            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsyncUser(null));
        }

    }
}
