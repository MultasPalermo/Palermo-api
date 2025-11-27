using Business.Interfaces.IBusinessImplements.Entities;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.Payments;
using Entity.DTOs.Select.Entities;
using Entity.DTOs.Select.EntitiesSelectDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using Entity.Infrastructure.Configurations.Payments;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Services.Entities
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly HttpClient _httpClient;
        private readonly MercadoPagoSettings _settings;
        private readonly ILogger<MercadoPagoService> _logger;

        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public string WebhookSecret => _settings.WebhookSecret;

        public MercadoPagoService(
            HttpClient httpClient,
            IOptions<MercadoPagoSettings> settings,
            ILogger<MercadoPagoService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        // ============================================================
        // 1) PAGO DE MULTA REAL (UserInfraction)
        // ============================================================
        public async Task<MercadoPagoPreferenceResult> CreateCheckoutForInfractionAsync(
       UserInfractionSelectDto multa,
       UserSelectDto usuario)
        {
            // 🟢 Calcular monto correctamente
            decimal amountToPay =
                multa.TotalToPay > 0 ? multa.TotalToPay :
                multa.amountToPay > 0 ? multa.amountToPay :
                multa.InitialAmount > 0 ? multa.InitialAmount :
                0m;

            // 🟢 El nombre REAL viene del UserInfractionSelectDto
            var payer = new
            {
                email = usuario.email ?? multa.userEmail,
                name = $"{multa.firstName} {multa.lastName}",  // ← AQUÍ ESTÁ LA SOLUCIÓN
                identification = new { type = "CC", number = usuario.documentNumber ?? multa.documentNumber }
            };

            var payload = new
            {
                items = new[]
                {
            new
            {
                title = $"Pago Multa #{multa.id:D6}",
                quantity = 1,
                unit_price = (int)Math.Round(amountToPay),
                currency_id = "COP"
            }
        },

                payer,

                back_urls = new
                {
                    success = _settings.SuccessUrl,
                    failure = _settings.FailureUrl,
                    pending = _settings.PendingUrl
                },

                auto_return = "approved",
                notification_url = _settings.NotificationUrl,

                external_reference = $"FINE-{multa.id}"
            };

            return await SendPreferenceAsync(payload, amountToPay);
        }

                _logger.LogInformation($"Creando preferencia de pago: UserInfractionId={userInfractionId}, Amount={amount}, Title={title}");

        // ============================================================
        // 2) PAGO DE UNA CUOTA DE ACUERDO DE PAGO
        // ============================================================
        public async Task<MercadoPagoPreferenceResult> CreateCheckoutForAgreementAsync(
       InstallmentScheduleSelectDto cuota,
       PaymentAgreementSelectDto acuerdo,
       UserSelectDto usuario)
        {
            // PAGADOR REAL — sale del ACUERDO, NO del usuario del sistema
            var payer = new
            {
                email = acuerdo.Email,
                name = acuerdo.PersonName,
                identification = new
                {
                    type = "CC",
                    number = acuerdo.DocumentNumber
                }
            };

            var payload = new
            {
                items = new[]
                {
            new
            {
                title = $"Cuota #{cuota.Id:D6} del Acuerdo #{acuerdo.Id:D6}",
                quantity = 1,
                unit_price = (int)Math.Round(cuota.Amount),
                currency_id = "COP"
            }
        },

                payer,

                back_urls = new
                {
                    success = _settings.SuccessUrl,
                    failure = _settings.FailureUrl,
                    pending = _settings.PendingUrl
                },
                    AutoReturn = "approved",
                    ExternalReference = userInfractionId.ToString(),
                    NotificationUrl = _configuration["MercadoPago:WebhookUrl"] ?? "https://tu-dominio.com/api/payment/webhook",
                    StatementDescriptor = "Pago de Multa",
                    BinaryMode = false
                };

                auto_return = "approved",
                notification_url = _settings.NotificationUrl,

                external_reference = $"AGREEMENT-{acuerdo.Id}-INSTALLMENT-{cuota.Id}"
            };

            return await SendPreferenceAsync(payload, cuota.Amount);
        }




        // ============================================================
        // FUNCIÓN COMÚN PARA ENVIAR PREFERENCIA
        // ============================================================
        private async Task<MercadoPagoPreferenceResult> SendPreferenceAsync(object payload, decimal amount)
        {
            using var content = new StringContent(
                JsonSerializer.Serialize(payload, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/checkout/preferences", content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("MercadoPago error creando preferencia: {Error}", responseText);
                throw new HttpRequestException($"MercadoPago error {response.StatusCode}: {responseText}");
            }

            var json = JsonDocument.Parse(responseText).RootElement;

            return new MercadoPagoPreferenceResult
            {
                PreferenceId = json.GetProperty("id").GetString()!,
                InitPoint = json.GetProperty("init_point").GetString()!,
                Amount = amount,
                Currency = _settings.DefaultCurrency
            };
        }

        // ============================================================
        // OBTENER INFORMACIÓN DE PAGO
        // ============================================================
        public async Task<MercadoPagoPaymentInfoDto?> GetPaymentAsync(string paymentId)
        {
            var response = await _httpClient.GetAsync($"/v1/payments/{paymentId}");
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Lookup de pago {PaymentId} falló: {Body}", paymentId, body);
                return null;
            }

            var json = JsonDocument.Parse(body).RootElement;

            return new MercadoPagoPaymentInfoDto
            {
                Id = json.TryGetProperty("id", out var id)
                    ? (id.ValueKind == JsonValueKind.String ? id.GetString() : id.GetRawText().Trim('"'))
                    : null,

                Status = json.TryGetProperty("status", out var status) ? status.GetString() : null,

                TransactionAmount = json.TryGetProperty("transaction_amount", out var amt)
                    ? amt.GetDecimal()
                    : null,

                ExternalReference = json.TryGetProperty("external_reference", out var ex)
                    ? (ex.ValueKind == JsonValueKind.String ? ex.GetString() : ex.GetRawText().Trim('"'))
                    : null
            };
        }

        // ============================================================
        // WEBHOOK SIGNATURE VALIDATION
        // ============================================================
        public bool ValidateWebhookSignature(string? signature, string payload)
        {
            if (string.IsNullOrWhiteSpace(_settings.WebhookSecret))
                return true;

            return signature == _settings.WebhookSecret;
        }
    }
}
