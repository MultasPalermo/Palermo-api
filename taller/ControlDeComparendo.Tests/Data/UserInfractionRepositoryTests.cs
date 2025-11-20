using Xunit;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Data.Services.Entities;
using Entity.Domain.Models.Implements.Entities;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.ModelSecurity;

public class UserInfractionRepository_GetById_Tests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    // =======================================================
    // PRUEBA 1 — No existe → retorna null
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_NotFound()
    {
        // Arrange
        var ctx = CreateContext();
        var repo = new UserInfractionRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    // =======================================================
    // PRUEBA 2 — Existe → retorna entidad
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_ReturnEntity_When_Found()
    {
        // Arrange
        var ctx = CreateContext();

        var user = new User
        {
            id = 1,
            Person = new Person { id = 100, firstName = "John" }
        };

        var infraction = new Infraction
        {
            id = 5,
            TypeInfraction = new TypeInfraction { id = 8 }
        };

        var entity = new UserInfraction
        {
            id = 10,
            User = user,
            UserId = user.id,
            Infraction = infraction,
            InfractionId = infraction.id
        };

        ctx.userInfraction.Add(entity);
        await ctx.SaveChangesAsync();

        var repo = new UserInfractionRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.id);
        Assert.NotNull(result.User);
        Assert.NotNull(result.User.Person);
        Assert.NotNull(result.Infraction);
        Assert.NotNull(result.Infraction.TypeInfraction);
    }

    // =======================================================
    // PRUEBA 3 — Includes cargan correctamente
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_Include_User_Infraction_TypeInfraction()
    {
        // Arrange
        var ctx = CreateContext();

        var user = new User
        {
            id = 2,
            Person = new Person { id = 200, firstName = "Jane" }
        };

        var infraction = new Infraction
        {
            id = 6,
            TypeInfraction = new TypeInfraction { id = 9 }
        };

        ctx.userInfraction.Add(new UserInfraction
        {
            id = 20,
            UserId = user.id,
            User = user,
            InfractionId = infraction.id,
            Infraction = infraction
        });

        await ctx.SaveChangesAsync();

        var repo = new UserInfractionRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(20);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.User);
        Assert.NotNull(result.User.Person);
        Assert.NotNull(result.Infraction);
        Assert.NotNull(result.Infraction.TypeInfraction);
    }
}

