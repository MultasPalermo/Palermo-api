using Moq;
using AutoMapper;
using Business.Services.Entities;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Models.Implements.Entities;

public class FineCalculationDetailService_GetById_Tests
{
    private readonly Mock<IFineCalculationDetailRepository> _repoMock;
    private readonly Mock<IValueSmldvRepository> _valueSmldvMock;
    private readonly Mock<IInfractionRepository> _typeInfractionMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly FineCalculationDetailService _service;

    public FineCalculationDetailService_GetById_Tests()
    {
        // ========== REPOS / DEPENDENCIAS MOCKEADAS ==========
        _repoMock = new Mock<IFineCalculationDetailRepository>();
        _valueSmldvMock = new Mock<IValueSmldvRepository>();
        _typeInfractionMock = new Mock<IInfractionRepository>();
        _mapperMock = new Mock<IMapper>();

        // ========== CREACIÓN DEL SERVICIO BAJO PRUEBA ==========
        _service = new FineCalculationDetailService(
            _repoMock.Object,
            _mapperMock.Object,
            _valueSmldvMock.Object,
            _typeInfractionMock.Object
        );
    }

    // =======================================================
    // PRUEBA 1 — ID inválido → excepción
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_Throw_When_Id_Is_Zero_Or_Negative()
    {
        // Arrange
        int invalidId = 0;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetByIdAsync(invalidId));
    }

    // =======================================================
    // PRUEBA 2 — No encontrado → retorna null
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_Not_Found()
    {
        // Arrange
        int id = 5;
        _repoMock.Setup(r => r.GetByIdAsync(id))
                 .ReturnsAsync((FineCalculationDetail)null!);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    // =======================================================
    // PRUEBA 3 — Retorna DTO cuando existe
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_ReturnDto_When_Exists()
    {
        // Arrange
        int id = 10;

        var entity = new FineCalculationDetail { id = id };
        var dto = new FineCalculationDetailSelectDto { id = id };

        _repoMock.Setup(r => r.GetByIdAsync(id))
                 .ReturnsAsync(entity);

        _mapperMock.Setup(m => m.Map<FineCalculationDetailSelectDto>(entity))
                   .Returns(dto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.id);

        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<FineCalculationDetailSelectDto>(entity), Times.Once);
    }
}
