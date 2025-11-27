using Azure.Core;
using Business.Interfaces.IBusinessImplements.Entities;
using Business.Services.Entities;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Web.Controllers.Implements.Entities
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IUserInfractionServices _infractionService;
        private readonly IPaymentAgreementServices _agreementService;
        private readonly IInstallmentScheduleServices _installmentService;
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            IUserInfractionServices infractionService,
            IPaymentAgreementServices agreementService,
            IInstallmentScheduleServices installmentService,
            IMercadoPagoService mercadoPagoService,
            ILogger<PaymentsController> logger)
        {
            _infractionService = infractionService;
            _agreementService = agreementService;
            _installmentService = installmentService;
            _mercadoPagoService = mercadoPagoService;
            _logger = logger;
        }

        // ============================================================
        // 1) CHECKOUT DE MULTA
        // ============================================================
        [HttpPost("infraction/{infractionId:int}/checkout")]
        public async Task<IActionResult> CreateCheckoutForInfraction(int infractionId)
        {
            var multa = await _infractionService.GetByIdAsync(infractionId);
            if (multa == null)
                return NotFound(new { message = $"La multa {infractionId} no existe." });

            var usuario = new UserSelectDto
            {
                email = multa.userEmail,
                documentNumber = multa.documentNumber
            };

            var pref = await _mercadoPagoService.CreateCheckoutForInfractionAsync(multa, usuario);

            return Ok(pref);
        }

        // ============================================================
        // 2) CHECKOUT DE CUOTA DE ACUERDO
        // ============================================================
        [HttpPost("agreement/{agreementId:int}/installment/{cuotaId:int}/checkout")]
        public async Task<IActionResult> CreateCheckoutForAgreement(int agreementId, int cuotaId)
        {
            var acuerdo = await _agreementService.GetByIdAsync(agreementId);
            if (acuerdo == null)
                return NotFound(new { message = $"Acuerdo {agreementId} no existe." });

            var cuota = await _installmentService.GetByIdAsync(cuotaId);
            if (cuota == null)
                return NotFound(new { message = $"La cuota {cuotaId} no existe." });

            var usuario = new UserSelectDto
            {
                email = acuerdo.Email,
                documentNumber = acuerdo.DocumentNumber
            };

            var pref = await _mercadoPagoService.CreateCheckoutForAgreementAsync(cuota, acuerdo, usuario);

            return Ok(new { url = pref.InitPoint });
        }

        // ============================================================
        // 3) WEBHOOK
        // ============================================================
        [HttpPost("mercadopago/webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhook()
        {
            // 1. Leer payload evitando errores cuando viene vacío (Swagger)
            Request.EnableBuffering();
            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawPayload = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            _logger.LogInformation("Webhook recibido: {Payload}", rawPayload);

            if (string.IsNullOrWhiteSpace(rawPayload))
            {
                _logger.LogWarning("⚠ Webhook vacío recibido.");
                return Ok();
            }

            // 2. Parsear JSON de manera segura
            JsonDocument json;
            try
            {
                json = JsonDocument.Parse(rawPayload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al parsear JSON del webhook");
                return Ok();
            }

            var root = json.RootElement;

            // 3. Extraer paymentId (MercadoPago puede mandar varios formatos)
            string? paymentId = null;

            if (root.TryGetProperty("resource", out var r))
                paymentId = r.GetString();

            if (paymentId == null &&
                root.TryGetProperty("data", out var d) &&
                d.TryGetProperty("id", out var id1))
            {
                paymentId = id1.GetString();
            }

            if (paymentId == null &&
                root.TryGetProperty("id", out var id2))
            {
                paymentId = id2.GetString();
            }

            if (paymentId == null)
            {
                _logger.LogWarning("⚠ Webhook recibido sin paymentId.");
                return Ok();
            }

            // 4. Consultar el pago real en MercadoPago
            var payment = await _mercadoPagoService.GetPaymentAsync(paymentId);
            if (payment == null || payment.Status?.ToLower() != "approved")
                return Ok();

            if (string.IsNullOrWhiteSpace(payment.ExternalReference))
                return Ok();

            var reference = payment.ExternalReference;

            // 5. Procesar MULTA o ACUERDO
            if (reference.StartsWith("FINE-"))
            {
                var idMulta = int.Parse(reference.Replace("FINE-", ""));
                await _infractionService.MarkAsPaidAsync(idMulta);
                _logger.LogInformation("✔ Multa {Id} marcada como pagada", idMulta);
            }
            else if (reference.StartsWith("AGREEMENT-"))
            {
                var parts = reference.Split('-');
                var cuotaId = int.Parse(parts[3]);

                await _installmentService.MarkInstallmentAsPaidAsync(cuotaId);
                _logger.LogInformation("✔ Cuota {Id} marcada como pagada", cuotaId);
            }

            return Ok();
        }
    }
}
