using Data.Services.Entities;
using Entity.Domain.Models.Implements.Entities;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class DocumentInfractionRepositoryTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    // ================================================
    // GET ALL
    // ================================================
    [Fact]
    public async Task GetAllAsync_Should_Return_Active_With_Relations()
    {
        // Arrange
        var ctx = CreateContext();

        var payment = new PaymentAgreement { id = 1 };
        var report = new InspectoraReport { id = 2 };

        ctx.documenInfraction.AddRange(
            new DocumentInfraction
            {
                id = 10,
                is_deleted = false,
                paymentAgreement = payment,
                PaymentAgreementId = payment.id,
                inspectoraReport = report,
                inspectoraReportId = report.id
            },
            new DocumentInfraction
            {
                id = 20,
                is_deleted = true
            }
        );

        await ctx.SaveChangesAsync();

        var repo = new DocumentInfractionRepository(ctx);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.Single(result);

        var item = result.First();
        Assert.Equal(10, item.id);
        Assert.NotNull(item.paymentAgreement);
        Assert.NotNull(item.inspectoraReport);
    }

    // ================================================
    // GET DELETES
    // ================================================
    [Fact]
    
    public async Task GetDeletes_Should_Return_Deleted_With_Relations()
    {
        // Arrange
        var ctx = CreateContext();

        var payment = new PaymentAgreement { id = 5 };

        var report = new InspectoraReport
        {
            id = 8,
            message = "Test report" // 👈 CAMPO REQUERIDO
        };

        ctx.documenInfraction.AddRange(
            new DocumentInfraction
            {
                id = 30,
                is_deleted = false
            },
            new DocumentInfraction
            {
                id = 40,
                is_deleted = true,
                paymentAgreement = payment,
                PaymentAgreementId = payment.id,
                inspectoraReport = report,
                inspectoraReportId = report.id
            }
        );

        await ctx.SaveChangesAsync();

        var repo = new DocumentInfractionRepository(ctx);

        // Act
        var result = await repo.GetDeletes();

        // Assert
        Assert.Single(result);

        var item = result.First();
        Assert.Equal(40, item.id);
        Assert.NotNull(item.paymentAgreement);
        Assert.NotNull(item.inspectoraReport);
    }


    // ================================================
    // GET BY ID - FOUND
    // ================================================
    [Fact]
    public async Task GetByIdAsync_Should_Return_Entity_With_Relations()
    {
        // Arrange
        var ctx = CreateContext();

        var payment = new PaymentAgreement { id = 1 };
        var report = new InspectoraReport { id = 2 };

        ctx.documenInfraction.Add(new DocumentInfraction
        {
            id = 55,
            paymentAgreement = payment,
            PaymentAgreementId = payment.id,
            inspectoraReport = report,
            inspectoraReportId = report.id
        });

        await ctx.SaveChangesAsync();

        var repo = new DocumentInfractionRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(55);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(55, result.id);
        Assert.NotNull(result.paymentAgreement);
        Assert.NotNull(result.inspectoraReport);
    }

    // ================================================
    // GET BY ID - NOT FOUND
    // ================================================
    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var ctx = CreateContext();
        var repo = new DocumentInfractionRepository(ctx);

        // Act
        var result = await repo.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }
}
