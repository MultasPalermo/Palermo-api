using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Business.Services.Entities;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Models.Implements.Entities;
using Utilities.Exceptions;

public class InfractionService_GetById_Tests
{
    private readonly Mock<IInfractionRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<InfractionService>> _loggerMock;

    private readonly InfractionService _service;

    public InfractionService_GetById_Tests()
    {
        // ========== DEPENDENCIAS MOCKEADAS ==========
        _repoMock = new Mock<IInfractionRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<InfractionService>>();

        // ========== CREACIÓN DEL SERVICIO ==========
        _service = new InfractionService(
            _repoMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    // =======================================================
    // PRUEBA 1 — ID inválido → BusinessException
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
    // PRUEBA 2 — No encontrado → retorna null
    // =======================================================
    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_Not_Found()
    {
        // Arrange
        int id = 5;

        _repoMock.Setup(r => r.GetByIdAsync(id))
                 .ReturnsAsync((Infraction)null!);

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

        var entity = new Infraction { id = id };
        var dto = new InfractionSelectDto { id = id };

        _repoMock.Setup(r => r.GetByIdAsync(id))
                 .ReturnsAsync(entity);

        _mapperMock.Setup(m => m.Map<InfractionSelectDto>(entity))
                   .Returns(dto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.id);

        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<InfractionSelectDto>(entity), Times.Once);
    }
}
