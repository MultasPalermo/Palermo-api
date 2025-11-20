using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Interfaces.PDF;
using Business.Mensajeria.Email.implements;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.AnexarMulta;
using Web.Controllers.Implements.Entities;
using Azure;
using Microsoft.Extensions.DependencyInjection;

public class UserInfractionControllerTests
{
    private readonly Mock<IUserInfractionServices> _serviceMock;
    private readonly Mock<ILogger<UserInfractionController>> _loggerMock;
    private readonly Mock<IPdfGeneratorService> _pdfMock;
    private readonly Mock<IServiceScopeFactory> _scopeMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ReminderEmailAppService> _reminderServiceMock;
    private readonly UserInfractionController _controller;

    public UserInfractionControllerTests()
    {
        _serviceMock = new Mock<IUserInfractionServices>();
        _loggerMock = new Mock<ILogger<UserInfractionController>>();
        _pdfMock = new Mock<IPdfGeneratorService>();
        _scopeMock = new Mock<IServiceScopeFactory>();
        _userServiceMock = new Mock<IUserService>();
        _reminderServiceMock = new Mock<ReminderEmailAppService>();

        _controller = new UserInfractionController(
            _serviceMock.Object,
            _loggerMock.Object,
            _pdfMock.Object,
            _scopeMock.Object,
            _userServiceMock.Object,
            _reminderServiceMock.Object
        );
    }

    // ============================================================
    // GET ALL
    // ============================================================
    [Fact]
    public async Task Get_Should_Return_Data()
    {
        var list = new List<UserInfractionSelectDto>()
        {
            new UserInfractionSelectDto { id = 1 },
            new UserInfractionSelectDto { id = 2 }
        };

        _serviceMock.Setup(s => s.GetAllAsync(GetAllType.GetAll))
            .ReturnsAsync(list);

        var response = await _controller.Get(GetAllType.GetAll) as OkObjectResult;

        Assert.NotNull(response);
        Assert.IsType<OkObjectResult>(response);
    }

    // ============================================================
    // GET BY ID
    // ============================================================
    [Fact]
    public async Task GetById_Should_Return_Item()
    {
        var dto = new UserInfractionSelectDto { id = 10 };

        _serviceMock.Setup(s => s.GetByIdAsync(10))
            .ReturnsAsync(dto);

        var result = await _controller.GetById(10) as OkObjectResult;

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(10, (result.Value as UserInfractionSelectDto).id);
    }

    // ============================================================
    // POST CREATE
    // ============================================================
    [Fact]
    public async Task Post_Should_Invoke_Service_Create()
    {
        var dto = new UserInfractionDto { id = 5 };

        _serviceMock.Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(dto);

        var response = await _controller.Post(dto);

        Assert.IsType<OkObjectResult>(response);
        _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    // ============================================================
    // UPDATE
    // ============================================================
    [Fact]
    public async Task Put_Should_Invoke_Update()
    {
        var dto = new UserInfractionDto { id = 1 };

        _serviceMock.Setup(s => s.UpdateAsync(dto))
            .ReturnsAsync(true);

        var result = await _controller.Put(1, dto);

        Assert.IsType<OkObjectResult>(result);
        _serviceMock.Verify(s => s.UpdateAsync(dto), Times.Once);
    }

    // ============================================================
    // DELETE
    // ============================================================
    [Fact]
    public async Task Delete_Should_Invoke_DeleteAsync()
    {
        _serviceMock.Setup(s => s.DeleteAsync(3, DeleteType.Persistent))
            .ReturnsAsync(true);

        var result = await _controller.Delete(3, DeleteType.Persistent);

        Assert.IsType<OkObjectResult>(result);
        _serviceMock.Verify(s => s.DeleteAsync(3, DeleteType.Persistent), Times.Once);
    }

    // ============================================================
    // RESTORE
    // ============================================================
    [Fact]
    public async Task Restore_Should_Invoke_RestaureAsync()
    {
        _serviceMock.Setup(s => s.RestoreLogical(3))
            .ReturnsAsync(true);

        var result = await _controller.RestoreLogical(3);

        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.RestoreLogical(3), Times.Once);
    }

    // ============================================================
    // GET BY DOCUMENT
    // ============================================================
    [Fact]
    public async Task GetByDocument_Should_Return_Data()
    {
        var list = new List<UserInfractionSelectDto>() { new UserInfractionSelectDto { id = 1 } };

        _serviceMock
            .Setup(s => s.GetByDocumentAsync(1, "123"))
            .ReturnsAsync(list);

        var response = await _controller.GetByDocument(1, "123") as OkObjectResult;

        Assert.NotNull(response);
    }

    // ============================================================
    // GET BY TYPE
    // ============================================================
    [Fact]
    public async Task GetByType_Should_Return_Data()
    {
        var list = new List<UserInfractionSelectDto>() { new UserInfractionSelectDto { id = 1 } };

        _serviceMock
            .Setup(s => s.GetByTypeInfractionAsync(4))
            .ReturnsAsync(list);

        var response = await _controller.GetByTypeInfraction(4) as OkObjectResult;

        Assert.NotNull(response);
    }

    // ============================================================
    // PERSON BY DOCUMENT
    // ============================================================
    [Fact]
    public async Task GetPersonByDocument_Should_Return_HasInfraction()
    {
        var dto = new UserInfractionSelectDto
        {
            id = 1,
            firstName = "John",
            lastName = "Doe",
            userEmail = "test@mail.com"
        };

        _serviceMock
            .Setup(s => s.GetFirstByDocumentAsync(1, "123"))
            .ReturnsAsync(dto);

        var result = await _controller.GetPersonByDocument(1, "123") as OkObjectResult;

        Assert.NotNull(result);
    }

    // ============================================================
    // CREATE WITH PERSON
    // ============================================================
    [Fact]
    public async Task CreateWithPerson_Should_Return_Ok()
    {
        var request = new CreateInfractionRequestDto
        {
            DocumentNumber = "321",
            DocumentTypeId = 1
        };


        var created = new UserInfractionSelectDto { id = 99 };

        _serviceMock.Setup(s => s.CreateWithPersonAsync(request))
            .ReturnsAsync(created);

        var result = await _controller.CreateWithPerson(request);

        Assert.IsType<OkObjectResult>(result);
    }

    // ============================================================
    // PDF DOWNLOAD
    // ============================================================
    [Fact]
    public async Task DownloadPdf_Should_Return_File()
    {
        var dto = new UserInfractionSelectDto { id = 20, firstName = "Test" };

        _serviceMock.Setup(s => s.GetByIdAsyncPdf(20))
            .ReturnsAsync(dto);

        _pdfMock.Setup(p => p.GeneratePdfAsync(dto))
            .ReturnsAsync(new byte[10]);

        var result = await _controller.DownloadContractPdf(20);

        Assert.IsType<FileContentResult>(result);
    }

    // ============================================================
    // REMINDER PDF (3,15,25,30 días)
    // ============================================================
    [Fact]
    public async Task ReminderPdf3Days_Should_Return_File()
    {
        var dto = new UserInfractionSelectDto { id = 5, firstName = "Joe" };

        _serviceMock.Setup(s => s.GetByIdAsyncPdf(5))
            .ReturnsAsync(dto);

        _pdfMock.Setup(p => p.GenerateReminderPdfAsync(dto, 1))
            .ReturnsAsync(new byte[10]);

        var result = await _controller.DownloadReminder3DaysPdf(5);

        Assert.IsType<FileContentResult>(result);
    }

    // ============================================================
    // FILTER MULTAS
    // ============================================================
    [Fact]
    public async Task FilterMultas_Should_Return_Ok()
    {
        // Arrange
        var list = new List<UserInfractionSelectDto>
    {
        new UserInfractionSelectDto { id = 1 }
    };

        _serviceMock
            .Setup(s => s.GetMultasAsync(1, 2, EstadoMulta.Pendiente))
            .ReturnsAsync(list);

        // Act
        var result = await _controller.FilterMultas(1, 2, EstadoMulta.Pendiente);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
        Assert.Equal(list, okResult.Value);

        _serviceMock.Verify(
            s => s.GetMultasAsync(1, 2, EstadoMulta.Pendiente),
            Times.Once);
    }

}