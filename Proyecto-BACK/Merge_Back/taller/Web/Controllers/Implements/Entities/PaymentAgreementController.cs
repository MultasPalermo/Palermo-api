using Business.Interfaces.IBusinessImplements.Entities;
using Business.Interfaces.PDF;
using Business.Services.PDF;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Select.Entities;
using Entity.Infrastructure.Contexts;
using Entity.Init;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Web.Controllers.ControllersBase.Web.Controllers.BaseController;

namespace Web.Controllers.Implements.Entities
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [Produces("application/json")]
    public class PaymentAgreementController
        : BaseController<PaymentAgreementDto, PaymentAgreementSelectDto, IPaymentAgreementServices>
    {
        private readonly IPaymentAgreementServices _paymentAgreementService;
        private readonly IPdfGeneratorService _pdfService;

        public PaymentAgreementController(
            IPaymentAgreementServices services,
            IPdfGeneratorService pdf,
            ILogger<PaymentAgreementController> logger
        ) : base(services, logger)
        {
            _pdfService = pdf;
            _paymentAgreementService = services; // ✅ ahora sí se asigna
        }

        [HttpGet("init/{userId:int}")]
        public async Task<IActionResult> GetInitData(int userId, [FromQuery] int? infractionId = null)
        {
            var data = await _paymentAgreementService.GetInitDataAsync(userId, infractionId);

            if (data == null || !data.Any())
                return NotFound("No se encontraron multas para este usuario.");

            if (infractionId.HasValue)
            {
                var multa = data.FirstOrDefault(x => x.InfractionId == infractionId.Value);
                if (multa == null) return NotFound("La multa no existe.");
                return Ok(multa);
            }

            // Si no se pasa infractionId → devuelve todas
            return Ok(data);
        }

        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] PaymentAgreementDto dto)
        {
            try
            {
                // 🚀 Crear el acuerdo
                var created = await _paymentAgreementService.CreateAsync(dto);

                if (created == null)
                    return BadRequest(new { message = "No se pudo crear el acuerdo de pago." });

                // 🔹 Generar URL del PDF
                var pdfUrl = Url.Action(
                     nameof(DownloadPaymentAgreementPdf),
                     "PaymentAgreement",
                     new { id = created.Id },
                     Request.Scheme,
                     Request.Host.ToString()
                );



                // 👉 Devolver el acuerdo y el link del PDF
                return Ok(new
                {
                    agreement = created,
                    pdfUrl
                });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al agregar acuerdo de pago");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar acuerdo de pago");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }


        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DownloadPaymentAgreementPdf(int id)
        {
            var paymentAgreementDto = await _paymentAgreementService.GetByIdAsync(id);
            if (paymentAgreementDto == null)
            {
                _logger.LogWarning("Acuerdo de pago con ID {Id} no encontrado.", id);
                return NotFound(new { message = $"No se encontró un acuerdo de pago con ID {id}" });
            }

            var pdfBytes = await _pdfService.GeneratePaymentAgreementPdfAsync(paymentAgreementDto);
            return File(pdfBytes, "application/pdf", $"AcuerdoPago_{id}.pdf");
        }

        protected override Task<IEnumerable<PaymentAgreementSelectDto>> GetAllAsync(GetAllType getAllType)
            => _service.GetAllAsync(getAllType);

        protected override Task<PaymentAgreementSelectDto?> GetByIdAsync(int id)
            => _service.GetByIdAsync(id);

        protected override Task AddAsync(PaymentAgreementDto dto)
            => _service.CreateAsync(dto);

        protected override Task<bool> UpdateAsync(int id, PaymentAgreementDto dto)
            => _service.UpdateAsync(dto);

        protected override Task<bool> DeleteAsync(int id, DeleteType deleteType)
            => _service.DeleteAsync(id, deleteType);

        protected override Task<bool> RestaureAsync(int id)
            => _service.RestoreLogical(id);
    }
}