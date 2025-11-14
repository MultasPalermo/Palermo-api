using Business.Services.Entities;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.EntitiesDto;
using Entity.Infrastructure.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly MercadoPagoService _mercadoPagoService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            ApplicationDbContext context,
            MercadoPagoService mercadoPagoService,
            ILogger<PaymentController> logger)
        {
            _context = context;
            _mercadoPagoService = mercadoPagoService;
            _logger = logger;
        }

        /// <summary>
        /// Verificar estado del sistema de pagos
        /// </summary>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            try
            {
                var paymentsCount = _context.Payment.Count();
                var accessToken = _mercadoPagoService != null ? "Configurado" : "No configurado";
                
                return Ok(new
                {
                    status = "OK",
                    database = "Conectada",
                    paymentsTable = "Existe",
                    paymentsCount = paymentsCount,
                    mercadoPago = accessToken,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "ERROR",
                    message = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Crear preferencia de pago para una multa
        /// </summary>
        [HttpPost("create-preference/{userInfractionId}")]
        public async Task<IActionResult> CreatePaymentPreference(int userInfractionId)
        {
            try
            {
                var infraction = await _context.userInfraction
                    .Include(ui => ui.User)
                        .ThenInclude(u => u.Person)
                    .Include(ui => ui.Infraction)
                    .FirstOrDefaultAsync(ui => ui.id == userInfractionId);

                if (infraction == null)
                    return NotFound(new { message = "Infracción no encontrada" });

                if (infraction.stateInfraction != EstadoMulta.Pendiente)
                    return BadRequest(new { message = "La infracción ya no está pendiente de pago" });

                // Crear preferencia en MercadoPago
                var userName = infraction.User?.Person != null 
                    ? $"{infraction.User.Person.firstName} {infraction.User.Person.lastName}"
                    : (infraction.User?.email ?? "Usuario");
                
                var userEmail = infraction.User?.email ?? "sin-email@dominio.com";
                    
                var preference = await _mercadoPagoService.CreatePaymentPreferenceAsync(
                    userInfractionId,
                    infraction.amountToPay,
                    $"Pago de Multa - {infraction.Infraction?.description ?? "Infracción"}",
                    userEmail,
                    userName
                );

                // Guardar el registro del pago
                var payment = new Payment
                {
                    UserInfractionId = userInfractionId,
                    Amount = infraction.amountToPay,
                    Currency = "COP",
                    PreferenceId = preference?.Id,
                    Status = PaymentStatus.Pending,
                    PayerEmail = userEmail,
                    PayerName = userName ?? "Usuario",
                    created_date = DateTime.UtcNow,
                    active = true
                };

                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    preferenceId = preference?.Id ?? "",
                    initPoint = preference?.InitPoint ?? "",
                    sandboxInitPoint = preference?.SandboxInitPoint ?? "",
                    amount = infraction.amountToPay,
                    paymentId = payment.id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear preferencia de pago para UserInfractionId: {userInfractionId}");
                return StatusCode(500, new { 
                    message = "Error al crear preferencia de pago", 
                    error = ex.Message,
                    details = ex.InnerException?.Message 
                });
            }
        }

        /// <summary>
        /// Webhook para notificaciones de MercadoPago
        /// </summary>
        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> MercadoPagoWebhook([FromQuery] string type, [FromQuery] long? id)
        {
            try
            {
                _logger.LogInformation($"Webhook recibido - Type: {type}, ID: {id}");

                if (type != "payment" || !id.HasValue)
                    return Ok();

                // Obtener información del pago desde MercadoPago
                var mpPayment = await _mercadoPagoService.GetPaymentAsync(id.Value);

                if (mpPayment == null)
                    return Ok();

                // Buscar el pago en nuestra base de datos
                var payment = await _context.Payment
                    .Include(p => p.UserInfraction)
                    .FirstOrDefaultAsync(p => p.MercadoPagoPaymentId == id.Value ||
                                             (!string.IsNullOrEmpty(mpPayment.ExternalReference) && p.PreferenceId == mpPayment.ExternalReference));

                if (payment == null)
                {
                    // Buscar por ExternalReference
                    if (!string.IsNullOrEmpty(mpPayment.ExternalReference) && int.TryParse(mpPayment.ExternalReference, out int userInfractionId))
                    {
                        payment = await _context.Payment
                            .Include(p => p.UserInfraction)
                            .FirstOrDefaultAsync(p => p.UserInfractionId == userInfractionId &&
                                                     p.Status == PaymentStatus.Pending);
                    }
                }

                if (payment != null)
                {
                    // Actualizar información del pago
                    payment.MercadoPagoPaymentId = mpPayment.Id;
                    payment.MercadoPagoStatus = mpPayment.Status ?? "unknown";
                    payment.MercadoPagoStatusDetail = mpPayment.StatusDetail ?? "";
                    payment.PaymentMethod = mpPayment.PaymentMethodId ?? "";
                    payment.TransactionDetails = JsonSerializer.Serialize(new
                    {
                        TransactionAmount = mpPayment.TransactionAmount,
                        Installments = mpPayment.Installments
                    });

                    // Actualizar estado según respuesta de MercadoPago
                    switch (mpPayment.Status ?? "unknown")
                    {
                        case "approved":
                            payment.Status = PaymentStatus.Approved;
                            payment.PaidAt = DateTime.UtcNow;
                            
                            // Actualizar estado de la infracción
                            if (payment.UserInfraction != null)
                            {
                                payment.UserInfraction.stateInfraction = EstadoMulta.Pagada;
                                payment.UserInfraction.StatusCollection = EstadoCobro.CobroPrejuridico;
                            }
                            break;

                        case "pending":
                        case "in_process":
                            payment.Status = PaymentStatus.InProcess;
                            break;

                        case "rejected":
                            payment.Status = PaymentStatus.Rejected;
                            break;

                        case "cancelled":
                            payment.Status = PaymentStatus.Cancelled;
                            break;

                        case "refunded":
                            payment.Status = PaymentStatus.Refunded;
                            if (payment.UserInfraction != null)
                            {
                                payment.UserInfraction.stateInfraction = EstadoMulta.Pendiente;
                            }
                            break;
                    }

                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Pago actualizado - ID: {payment.id}, Status: {payment.Status}");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando webhook de MercadoPago");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Obtener estado de un pago
        /// </summary>
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentStatus(int paymentId)
        {
            try
            {
                var payment = await _context.Payment
                    .Include(p => p.UserInfraction)
                        .ThenInclude(ui => ui.Infraction)
                    .FirstOrDefaultAsync(p => p.id == paymentId);

                if (payment == null)
                    return NotFound(new { message = "Pago no encontrado" });

                return Ok(new
                {
                    payment.id,
                    payment.Amount,
                    payment.Status,
                    statusDescription = payment.Status.ToString(),
                    payment.PaidAt,
                    payment.MercadoPagoPaymentId,
                    payment.PaymentMethod,
                    infraction = new
                    {
                        payment.UserInfraction.id,
                        payment.UserInfraction.stateInfraction,
                        description = payment.UserInfraction.Infraction.description
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estado del pago");
                return StatusCode(500, new { message = "Error al obtener estado del pago" });
            }
        }

        /// <summary>
        /// Obtener historial de pagos de un usuario
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPayments(int userId)
        {
            try
            {
                var payments = await _context.Payment
                    .Include(p => p.UserInfraction)
                        .ThenInclude(ui => ui.Infraction)
                    .Where(p => p.UserInfraction.UserId == userId)
                    .OrderByDescending(p => p.created_date)
                    .Select(p => new
                    {
                        p.id,
                        p.Amount,
                        p.Status,
                        p.PaidAt,
                        p.PaymentMethod,
                        p.created_date,
                        infraction = new
                        {
                            p.UserInfraction.id,
                            description = p.UserInfraction.Infraction.description,
                            p.UserInfraction.dateInfraction
                        }
                    })
                    .ToListAsync();

                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pagos del usuario");
                return StatusCode(500, new { message = "Error al obtener pagos del usuario" });
            }
        }
    }
}
