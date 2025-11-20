using Xunit;
using Microsoft.EntityFrameworkCore;
using Entity.Infrastructure.Contexts;
using Data.Services.Entities;
using Entity.Domain.Models.Implements.Entities;
using System.Threading.Tasks;

public class FineCalculationDetailsRepository_GetById_Tests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
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
        var repo = new FineCalculationDetailsRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }


    // =======================================================
    // PRUEBA 3 — Includes funcionan correctamente
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_Include_All_Relations()
    {
        // Arrange
        var ctx = CreateContext();

        var smldv = new ValueSmldv
        {
            id = 5,
            value_smldv = 5000
        };

        var typeInf = new TypeInfraction
        {
            id = 40,
            Name = "Tipo Relación"
        };

        var infr = new Infraction
        {
            id = 50,
            TypeInfraction = typeInf,
            TypeInfractionId = typeInf.id
        };

        ctx.fineCalculationDetail.Add(new FineCalculationDetail
        {
            id = 99,
            valueSmldv = smldv,
            valueSmldvId = smldv.id,

            // 🔥 CORRECCIÓN OBLIGATORIA
            Infraction = infr,
            typeInfractionId = infr.id
        });

        await ctx.SaveChangesAsync();

        var repo = new FineCalculationDetailsRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(99);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.valueSmldv);
        Assert.NotNull(result.Infraction);
        Assert.NotNull(result.Infraction.TypeInfraction);
        Assert.Equal("Tipo Relación", result.Infraction.TypeInfraction.Name);
    }

}

