using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Business.Services.Entities;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Models.Implements.Entities;
using Utilities.Exceptions;

public class DocumentInfractionServices_GetById_Tests
{
    private readonly Mock<IDocumentInfractionRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<DocumentInfractionServices>> _loggerMock;
    private readonly DocumentInfractionServices _service;

    public DocumentInfractionServices_GetById_Tests()
    {
        // ========== REPOS / DEPENDENCIAS MOCKEADAS =============
        _repoMock = new Mock<IDocumentInfractionRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<DocumentInfractionServices>>();

        // Context InMemory (porque ExistsAsync usa EF Core)
        var ctx = TestDbContextFactory.Create();

        // ========== CREACIÓN DEL SERVICIO BAJO PRUEBA ==========
        _service = new DocumentInfractionServices(
            _repoMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            ctx
        );
    }

    // =======================================================
    // PRUEBA 1 — ID inválido
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_Throw_When_Id_Is_Zero_Or_Negative()
    {
        // Arrange
        int invalidId = 0;

        // Act & Assert
        await Assert.ThrowsAsync<BusinessException>(() => _service.GetByIdAsync(invalidId));
    }

    // =======================================================
    // PRUEBA 3 — Retorna DTO cuando existe
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_ReturnDto_When_Exists()
    {
        // Arrange
        var ctx = TestDbContextFactory.Create();

        var entityInDb = new DocumentInfraction { id = 10 };

        // Usamos Set<T> para evitar problemas con nombres de DbSets
        ctx.Set<DocumentInfraction>().Add(entityInDb);
        await ctx.SaveChangesAsync();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<DocumentInfractionSelectDto>(entityInDb))
                  .Returns(new DocumentInfractionSelectDto { id = 10 });

        var service = new DocumentInfractionServices(
            _repoMock.Object,
            mapperMock.Object,
            _loggerMock.Object,
            ctx
        );

        var entity = new DocumentInfraction { id = 10 };
        var dto = new DocumentInfractionSelectDto { id = 10 };

        _repoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);
        mapperMock.Setup(m => m.Map<DocumentInfractionSelectDto>(entity)).Returns(dto);

        // Act
        var result = await service.GetByIdAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.id);

        _repoMock.Verify(r => r.GetByIdAsync(10), Times.Once);
        mapperMock.Verify(m => m.Map<DocumentInfractionSelectDto>(entity), Times.Once);
    }

}
