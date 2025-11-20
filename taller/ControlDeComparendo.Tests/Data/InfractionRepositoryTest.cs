using Xunit;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Data.Services.Entities;
using Entity.Domain.Models.Implements.Entities;
using System.Threading.Tasks;

public class InfractionRepository_GetById_Tests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    // =======================================================
    // PRUEBA 1 — No existe → null
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_NotFound()
    {
        // Arrange
        var ctx = CreateContext();
        var repo = new InfractionRepository(ctx);

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

        var typeInf = new TypeInfraction
        {
            id = 15,
            Name = "Test TI"   // ← CORRECTO según tu entidad real
        };

        var entity = new Infraction
        {
            id = 10,
            TypeInfraction = typeInf,
            TypeInfractionId = typeInf.id
        };

        ctx.Infraction.Add(entity);   // ← CORRECTO: tu DbSet real es "infraction"
        await ctx.SaveChangesAsync();

        var repo = new InfractionRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.id);
        Assert.NotNull(result.TypeInfraction);
        Assert.Equal(15, result.TypeInfraction.id);
        Assert.Equal("Test TI", result.TypeInfraction.Name);
    }


    // =======================================================
    // PRUEBA 3 — Includes funcionan correctamente
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_Include_TypeInfraction()
    {
        // Arrange
        var ctx = CreateContext();

        var type = new TypeInfraction
        {
            id = 20,
            Name = "Incluido"
        };

        ctx.Infraction.Add(new Infraction
        {
            id = 30,
            TypeInfraction = type,
            TypeInfractionId = type.id
        });

        await ctx.SaveChangesAsync();

        var repo = new InfractionRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(30);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.TypeInfraction);
        Assert.Equal(20, result.TypeInfraction.id);
        Assert.Equal("Incluido", result.TypeInfraction.Name);
    }

}
