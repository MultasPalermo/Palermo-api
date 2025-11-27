using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using Entity.Domain.Enums;
using Web.Controllers.Implements.Entities;
using Business.Interfaces.IBusinessImplements.Entities;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class DocumentInfractionControllerTests
{
    private readonly Mock<IDocumentInfractionServices> _serviceMock;
    private readonly Mock<ILogger<DocumentInfractionController>> _loggerMock;
    private readonly DocumentInfractionController _controller;

    public DocumentInfractionControllerTests()
    {
        _serviceMock = new Mock<IDocumentInfractionServices>();
        _loggerMock = new Mock<ILogger<DocumentInfractionController>>();

        _controller = new DocumentInfractionController(
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
        var list = new List<DocumentInfractionSelectDto>
        {
            new DocumentInfractionSelectDto { id = 1 },
            new DocumentInfractionSelectDto { id = 2 }
        };

        _serviceMock.Setup(s => s.GetAllAsync(GetAllType.GetAll))
                    .ReturnsAsync(list);

        // Act
        var result = await _controller.Get(GetAllType.GetAll);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<IEnumerable<DocumentInfractionSelectDto>>(ok.Value);

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
        var dto = new DocumentInfractionSelectDto { id = 10 };

        _serviceMock.Setup(s => s.GetByIdAsync(10)).ReturnsAsync(dto);

        // Act
        var result = await _controller.GetById(10);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<DocumentInfractionSelectDto>(ok.Value);

        Assert.Equal(10, data.id);
        _serviceMock.Verify(s => s.GetByIdAsync(10), Times.Once);
    }

    // =======================================================
    // POST
    // =======================================================
    [Fact]
    public async Task Post_Should_Call_Service_Create()
    {
        // Arrange
        var dto = new DocumentInfractionDto { id = 1 };
        _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(dto);

        // Act
        var result = await _controller.Post(dto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    // =======================================================
    // PUT
    // =======================================================
    [Fact]
    public async Task Put_Should_Update()
    {
        var dto = new DocumentInfractionDto { id = 22 };
        _serviceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(true);

        var result = await _controller.Put(22, dto);

        Assert.IsType<OkObjectResult>(result);
        _serviceMock.Verify(s => s.UpdateAsync(dto), Times.Once);
    }

    // =======================================================
    // DELETE
    // =======================================================
    [Fact]
    public async Task Delete_Should_Call_Service()
    {
        _serviceMock.Setup(s => s.DeleteAsync(44, DeleteType.Logical))
                    .ReturnsAsync(true);

        var result = await _controller.Delete(44, DeleteType.Logical);

        Assert.IsType<OkObjectResult>(result);
        _serviceMock.Verify(s => s.DeleteAsync(44, DeleteType.Logical), Times.Once);
    }

    // =======================================================
    // RESTORE LOGICAL
    // =======================================================
    [Fact]
    public async Task RestoreLogical_Should_Call_Service()
    {
        _serviceMock.Setup(s => s.RestoreLogical(33)).ReturnsAsync(true);

        var result = await _controller.RestoreLogical(33);

        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.RestoreLogical(33), Times.Once);
    }
}

