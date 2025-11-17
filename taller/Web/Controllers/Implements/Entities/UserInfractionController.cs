using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Interfaces.PDF;
using Business.Mensajeria.Email.implements;
using Business.Mensajeria.Email.@interface;
using Business.Services.Security;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.AnexarMulta;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements.Entities
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserInfractionController : BaseController<UserInfractionDto, UserInfractionSelectDto, IUserInfractionServices>
    {
        private readonly IPdfGeneratorService _pdfService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IUserService _users;
        private readonly ReminderEmailAppService _reminderService;
        private readonly IUserInfractionServices _userInfractionServices;
        

        public UserInfractionController(
            IUserInfractionServices services,
            ILogger<UserInfractionController> logger,
            IPdfGeneratorService pdf,
            IServiceScopeFactory scopeFactory,
            IUserService users,
            ReminderEmailAppService reminderService
        ) : base(services, logger)
        {
            _pdfService = pdf;
            _scopeFactory = scopeFactory;
            _users = users;
            _reminderService = reminderService;
            _userInfractionServices = services;

        }

        protected override Task<IEnumerable<UserInfractionSelectDto>> GetAllAsync(GetAllType getAllType)
            => _service.GetAllAsync(getAllType);

        protected override Task<UserInfractionSelectDto?> GetByIdAsync(int id)
            => _service.GetByIdAsync(id);

        protected override Task AddAsync(UserInfractionDto dto)
            => _service.CreateAsync(dto);

        protected override Task<bool> UpdateAsync(int id, UserInfractionDto dto)
            => _service.UpdateAsync(dto);

        protected override Task<bool> DeleteAsync(int id, DeleteType deleteType)
            => _service.DeleteAsync(id, deleteType);

        protected override Task<bool> RestaureAsync(int id)
            => _service.RestoreLogical(id);

        // Consulta por documento
        [HttpGet("by-document")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> GetByDocument([FromQuery] int documentTypeId, [FromQuery] string documentNumber)
        {
            if (documentTypeId <= 0 || string.IsNullOrWhiteSpace(documentNumber))
                return BadRequest(new { isSuccess = false, message = "Parámetros inválidos." });

            var items = await _service.GetByDocumentAsync(documentTypeId, documentNumber.Trim());
            return Ok(new { isSuccess = true, count = items.Count(), data = items });
        }

        // Consulta por tipo de infracción
        [HttpGet("by-type")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> GetByTypeInfraction([FromQuery] int typeInfractionId)
        {
            if (typeInfractionId <= 0)
                return BadRequest(new { isSuccess = false, message = "Parámetro inválido." });

            var items = await _service.GetByTypeInfractionAsync(typeInfractionId);
            return Ok(new { isSuccess = true, count = items.Count(), data = items });
        }

        [HttpGet("person-by-document")]
        public async Task<IActionResult> GetPersonByDocument([FromQuery] int documentTypeId, [FromQuery] string documentNumber)
        {
            if (documentTypeId <= 0 || string.IsNullOrWhiteSpace(documentNumber))
                return BadRequest(new { isSuccess = false, message = "Parámetros inválidos." });

            var userInfraction = await _service.GetFirstByDocumentAsync(documentTypeId, documentNumber.Trim());

            if (userInfraction == null)
                return Ok(new { isSuccess = true, hasInfraction = false });

            return Ok(new
            {
                isSuccess = true,
                hasInfraction = true,
                data = new
                {
                    firstName = userInfraction.firstName,
                    lastName = userInfraction.lastName,
                    email = userInfraction.userEmail
                }
            });
        }


        [HttpPost("create-with-person")]
        [ProducesResponseType(typeof(UserInfractionSelectDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateWithPerson([FromBody] CreateInfractionRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { isSuccess = false, message = "Datos inválidos." });

            try
            {
                var result = await _service.CreateWithPersonAsync(dto);

                // ✅ Construir URL absoluta al PDF
                var pdfUrl = Url.Action(
                    nameof(DownloadContractPdf),     // acción del controlador
                    "UserInfraction",                // controlador
                    new { id = result.id },          // parámetro
                    Request.Scheme                   // http o https
                );

                return Ok(new
                {
                    isSuccess = true,
                    message = "Multa registrada exitosamente.",
                    data = result,
                    pdfUrl // 👈 ahora se envía la ruta del PDF
                });
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al crear multa con persona");
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear multa con persona");
                return StatusCode(500, new { isSuccess = false, message = "Error interno del servidor." });
            }
        }


        // GET: api/UserInfraction/{id}/pdf
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DownloadContractPdf(int id)
        {
            var userInfractionSelectDto = await _service.GetByIdAsyncPdf(id);
            if (userInfractionSelectDto == null)
            {
                _logger.LogWarning("Contrato con ID {Id} no encontrado.", id);
                return NotFound(new { message = $"No se encontró un contrato con ID {id}" });
            }

            var pdfBytes = await _pdfService.GeneratePdfAsync(userInfractionSelectDto);
            return File(pdfBytes, "application/pdf", $"Contrato_{userInfractionSelectDto.firstName}.pdf");
        }

        // ===========================================================
        // 📄 Descargar recordatorio de 3 días
        // ===========================================================
        [HttpGet("{id}/pdf/3dias")]
        public async Task<IActionResult> DownloadReminder3DaysPdf(int id)
        {
            return await GenerateAndReturnReminderPdf(id, 1, $"Recordatorio_3dias_{id}.pdf");
        }

        // ===========================================================
        // 📄 Descargar recordatorio de 15 días
        // ===========================================================
        [HttpGet("{id}/pdf/15dias")]
        public async Task<IActionResult> DownloadReminder15DaysPdf(int id)
        {
            return await GenerateAndReturnReminderPdf(id, 2, $"Recordatorio_15dias_{id}.pdf");
        }

        // ===========================================================
        // 📄 Descargar recordatorio de 25 días
        // ===========================================================
        [HttpGet("{id}/pdf/25dias")]
        public async Task<IActionResult> DownloadReminder25DaysPdf(int id)
        {
            return await GenerateAndReturnReminderPdf(id, 3, $"Recordatorio_25dias_{id}.pdf");
        }

        // ===========================================================
        // 📄 Descargar recordatorio de 30 días (cobro jurídico)
        // ===========================================================
        [HttpGet("{id}/pdf/cobroJuridico")]
        public async Task<IActionResult> DownloadReminder30DaysPdf(int id)
        {
            return await GenerateAndReturnReminderPdf(id, 4, $"CobroJuridico_{id}.pdf");
        }



        // ===========================================================
        // MÉTODO INTERNO REUTILIZABLE (SRP)
        // ===========================================================
        private async Task<IActionResult> GenerateAndReturnReminderPdf(int id, int reminderId, string fileName)
        {
            var userInfraction = await _service.GetByIdAsyncPdf(id);

            if (userInfraction == null)
                return NotFound(new { message = $"No se encontró una infracción con id {id}" });

            // Generate PDF using your new service and logic
            var pdfBytes = await _pdfService.GenerateReminderPdfAsync(userInfraction, reminderId);

            // Si el recordatorio NO tiene PDF configurado en GetReminderTemplateById()
            if (pdfBytes == null)
                return BadRequest(new { message = "Este tipo de recordatorio no genera PDF." });

            return File(pdfBytes, "application/pdf", fileName);
        }

        [HttpPost("simulate-interest")]
        public async Task<IActionResult> SimulateInterest([FromQuery] int days)
        {
            if (days < 0)
                return BadRequest("Los días deben ser positivos.");

            // Simular fecha avanzando días
            var simulatedDate = DateTime.UtcNow.AddDays(days);

            // Llamar al servicio que aplica intereses
            int updated = await _userInfractionServices.ApplyInterestToInfractionsAsync(simulatedDate);

            return Ok(new
            {
                isSuccess = true,
                simulatedDate,
                updatedRecords = updated
            });
        }




        //[HttpPost("test-email")]
        //public async Task<IActionResult> TestEmail([FromServices] ReminderEmailAppService service)
        //{
        //    var dto = new UserInfractionSelectDto
        //    {
        //        id = 12345,
        //        firstName = "Camilo",
        //        lastName = "Andrés",
        //        userEmail = "camiloandreslosada801@gmail.com",
        //        dateInfraction = DateTime.Now.AddDays(-5),
        //        amountToPay = 250000
        //    };

        //    await service.ProgramarRecordatoriosAsync(dto);
        //    return Ok("📨 Recordatorios programados (ver logs para el resultado).");
        //}


        //[HttpPost("test-send-email")]
        //public async Task<IActionResult> TestSendEmail([FromBody] UserInfractionDto dto)
        //{
        //    try
        //    {
        //        // 1️⃣ Crear la multa y obtener el DTO resultante con ID generado
        //        var createdDto = await _service.CreateAsync(dto);

        //        // 2️⃣ Obtener la multa completa con todos los datos necesarios para PDF
        //        var created = await _service.GetByIdAsyncPdf(createdDto.id);
        //        if (created == null)
        //            return NotFound(new { message = "No se pudo obtener la infracción creada." });

        //        // 3️⃣ Obtener el usuario (solo para email)
        //        var user = await _users.GetByIdAsync(dto.userId);
        //        if (user == null || string.IsNullOrEmpty(user.email))
        //            return NotFound(new { message = "Usuario no encontrado o sin email." });

        //        // 4️⃣ Generar PDF
        //        var pdfBytes = await _pdfService.GeneratePdfAsync(created);
        //        if (pdfBytes == null || pdfBytes.Length == 0)
        //            return StatusCode(500, new { message = "Error generando el PDF." });

        //        // 5️⃣ Construir y enviar email
        //        var builder = new InfraccionEmailBuilder(created, pdfBytes);

        //        using var scope = _scopeFactory.CreateScope();
        //        var emailService = scope.ServiceProvider.GetRequiredService<IServiceEmail>();

        //        await emailService.SendEmailAsync(
        //            user.email,
        //            builder.GetSubject(),
        //            builder.GetBody(),
        //            builder.GetAttachments()?.ToList()
        //        );

        //        // ✅ Devuelve la multa creada para verificación en Postman
        //        return Ok(created);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creando la infracción y enviando correo");
        //        return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        //    }
        //}


    }

}

