using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Implements.Entities;
using Business.Interfaces.IBusinessImplements.Entities;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InfractionControllerTests
{
    private readonly Mock<IInfractionService> _serviceMock;
    private readonly Mock<ILogger<InfractionController>> _loggerMock;
    private readonly InfractionController _controller;

    public InfractionControllerTests()
    {
        _serviceMock = new Mock<IInfractionService>();
        _loggerMock = new Mock<ILogger<InfractionController>>();

        _controller = new InfractionController(
            _serviceMock.Object,
            _loggerMock.Object
        );
    }

    // =======================================================
    // GET ALL
    // =======================================================
    [Fact]
    public async Task Get_Should_Return_Ok_With_Data()
    {
        // Arrange
        var resultList = new List<InfractionSelectDto>
        {
            new InfractionSelectDto { id = 1 },
            new InfractionSelectDto { id = 2 }
        };

        _serviceMock.Setup(s => s.GetAllAsync(GetAllType.GetAll))
                    .ReturnsAsync(resultList);

        // Act
        var response = await _controller.Get(GetAllType.GetAll);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(response);
        var data = Assert.IsAssignableFrom<IEnumerable<InfractionSelectDto>>(ok.Value);

        Assert.Equal(2, data.Count());
        _serviceMock.Verify(s => s.GetAllAsync(GetAllType.GetAll), Times.Once);
    }

    // =======================================================
    // GET BY ID
    // =======================================================
    [Fact]
    public async Task GetById_Should_Return_Ok_When_Found()
    {
        // Arrange
        var dto = new InfractionSelectDto { id = 10 };

        _serviceMock.Setup(s => s.GetByIdAsync(10))
                    .ReturnsAsync(dto);

        // Act
        var response = await _controller.GetById(10);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsType<InfractionSelectDto>(ok.Value);

        Assert.Equal(10, result.id);
        _serviceMock.Verify(s => s.GetByIdAsync(10), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Null()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(99))
                    .ReturnsAsync((InfractionSelectDto)null);

        var response = await _controller.GetById(99);

        Assert.IsType<NotFoundObjectResult>(response);
    }

    // =======================================================
    // POST (CREATE)
    // =======================================================
    [Fact]
    public async Task Post_Should_Invoke_Service_Create()
    {
        // Arrange
        var dto = new InfractionDto { id = 5 };

        _serviceMock.Setup(s => s.CreateAsync(dto))
                    .ReturnsAsync(dto);

        // Act
        var response = await _controller.Post(dto);

        // Assert
        Assert.IsType<OkObjectResult>(response);
        _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    // =======================================================
    // PUT (UPDATE)
    // =======================================================
    [Fact]
    public async Task Put_Should_Invoke_Service_Update()
    {
        var dto = new InfractionDto { id = 22 };

        _serviceMock.Setup(s => s.UpdateAsync(dto))
                    .ReturnsAsync(true);

        var response = await _controller.Put(22, dto);

        var ok = Assert.IsType<OkObjectResult>(response);

        _serviceMock.Verify(s => s.UpdateAsync(dto), Times.Once);
    }

    // =======================================================
    // DELETE
    // =======================================================
    [Fact]
    public async Task Delete_Should_Invoke_Service_Delete()
    {
        _serviceMock.Setup(s => s.DeleteAsync(33, DeleteType.Logical))
                    .ReturnsAsync(true);

        var response = await _controller.Delete(33, DeleteType.Logical);

        Assert.IsType<OkObjectResult>(response);

        _serviceMock.Verify(
            s => s.DeleteAsync(33, DeleteType.Logical),
            Times.Once
        );
    }

    // =======================================================
    // RESTORE
    // =======================================================
    [Fact]
    public async Task Restore_Should_Invoke_Service_RestoreLogical()
    {
        _serviceMock.Setup(s => s.RestoreLogical(44))
                    .ReturnsAsync(true);

        var response = await _controller.RestoreLogical(44);

        Assert.IsType<NoContentResult>(response);

        _serviceMock.Verify(s => s.RestoreLogical(44), Times.Once);
    }
}
