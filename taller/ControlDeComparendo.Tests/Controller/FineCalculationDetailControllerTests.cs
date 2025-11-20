using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Business.Interfaces.IBusinessImplements.Entities;
using Web.Controllers.Implements.Entities;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.AspNetCore.Mvc;
using Entity.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

public class FineCalculationDetailControllerTests
{
    private readonly Mock<IFineCalculationDetailService> _serviceMock;
    private readonly Mock<ILogger<FineCalculationDetailController>> _loggerMock;
    private readonly FineCalculationDetailController _controller;

    public FineCalculationDetailControllerTests()
    {
        _serviceMock = new Mock<IFineCalculationDetailService>();
        _loggerMock = new Mock<ILogger<FineCalculationDetailController>>();

        _controller = new FineCalculationDetailController(
            _serviceMock.Object,
            _loggerMock.Object
        );
    }

    // =======================================================
    // GET ALL
    // =======================================================
    [Fact]
    public async Task Get_Should_Return_List()
    {
        // Arrange
        var list = new List<FineCalculationDetailSelectDto>
        {
            new FineCalculationDetailSelectDto { id = 1 },
            new FineCalculationDetailSelectDto { id = 2 }
        };

        _serviceMock.Setup(s => s.GetAllAsync(GetAllType.GetAll))
                    .ReturnsAsync(list);

        // Act
        var result = await _controller.Get(GetAllType.GetAll);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<IEnumerable<FineCalculationDetailSelectDto>>(ok.Value);

        Assert.Equal(2, data.Count());
        _serviceMock.Verify(s => s.GetAllAsync(GetAllType.GetAll), Times.Once);
    }

    // =======================================================
    // GET BY ID
    // =======================================================
    [Fact]
    public async Task GetById_Should_Return_Element()
    {
        // Arrange
        var dto = new FineCalculationDetailSelectDto { id = 10 };

        _serviceMock.Setup(s => s.GetByIdAsync(10))
                    .ReturnsAsync(dto);

        // Act
        var result = await _controller.GetById(10);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<FineCalculationDetailSelectDto>(ok.Value);

        Assert.Equal(10, data.id);
        _serviceMock.Verify(s => s.GetByIdAsync(10), Times.Once);
    }


    // =======================================================
    // PUT
    // =======================================================
    [Fact]
    public async Task Put_Should_Invoke_Update()
    {
        var dto = new FineCalculationDetailDto { id = 21 };

        _serviceMock.Setup(s => s.UpdateAsync(dto))
                    .ReturnsAsync(true);

        // Act
        var result = await _controller.Put(21, dto);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        _serviceMock.Verify(s => s.UpdateAsync(dto), Times.Once);
    }

    // =======================================================
    // DELETE
    // =======================================================
    [Fact]
    public async Task Delete_Should_Invoke_Service_Delete()
    {
        _serviceMock.Setup(s => s.DeleteAsync(30, DeleteType.Logical))
                    .ReturnsAsync(true);

        var result = await _controller.Delete(30, DeleteType.Logical);

        Assert.IsType<OkObjectResult>(result);
        _serviceMock.Verify(s => s.DeleteAsync(30, DeleteType.Logical), Times.Once);
    }

    // =======================================================
    // RESTORE (PATCH)
    // =======================================================
    [Fact]
    public async Task RestoreLogical_Should_Invoke_Service_Restore()
    {
        _serviceMock.Setup(s => s.RestoreLogical(42))
                    .ReturnsAsync(true);

        var result = await _controller.RestoreLogical(42);

        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.RestoreLogical(42), Times.Once);
    }
}
