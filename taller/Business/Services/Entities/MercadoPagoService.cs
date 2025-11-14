using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Services.Entities
{
    public class MercadoPagoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MercadoPagoService> _logger;

        public MercadoPagoService(IConfiguration configuration, ILogger<MercadoPagoService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            
            // Configurar Access Token de MercadoPago
            var accessToken = _configuration["MercadoPago:AccessToken"];
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("MercadoPago AccessToken no configurado");
            }
            
            MercadoPagoConfig.AccessToken = accessToken;
        }

        public async Task<Preference> CreatePaymentPreferenceAsync(
            int userInfractionId,
            decimal amount,
            string title,
            string payerEmail,
            string? payerName = null)
        {
            try
            {
                if (amount <= 0)
                {
                    throw new ArgumentException("El monto debe ser mayor a cero", nameof(amount));
                }

                if (string.IsNullOrWhiteSpace(title))
                {
                    throw new ArgumentException("El título es requerido", nameof(title));
                }

                _logger.LogInformation($"Creando preferencia de pago: UserInfractionId={userInfractionId}, Amount={amount}, Title={title}");

                var request = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
                    {
                        new PreferenceItemRequest
                        {
                            Title = title,
                            Quantity = 1,
                            CurrencyId = "COP",
                            UnitPrice = amount
                        }
                    },
                    Payer = new PreferencePayerRequest
                    {
                        Email = payerEmail ?? "sin-email@dominio.com",
                        Name = payerName ?? "Usuario"
                    },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = _configuration["MercadoPago:SuccessUrl"] ?? "https://tu-dominio.com/payment/success",
                        Failure = _configuration["MercadoPago:FailureUrl"] ?? "https://tu-dominio.com/payment/failure",
                        Pending = _configuration["MercadoPago:PendingUrl"] ?? "https://tu-dominio.com/payment/pending"
                    },
                    AutoReturn = "approved",
                    ExternalReference = userInfractionId.ToString(),
                    NotificationUrl = _configuration["MercadoPago:WebhookUrl"] ?? "https://tu-dominio.com/api/payment/webhook",
                    StatementDescriptor = "Pago de Multa",
                    BinaryMode = false
                };

                var client = new PreferenceClient();
                var preference = await client.CreateAsync(request);

                _logger.LogInformation($"Preferencia de pago creada: {preference.Id}");
                
                return preference;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear preferencia de pago en MercadoPago. UserInfractionId: {userInfractionId}, Amount: {amount}");
                throw new InvalidOperationException($"Error al crear preferencia de pago: {ex.Message}", ex);
            }
        }

        public async Task<Payment> GetPaymentAsync(long paymentId)
        {
            try
            {
                var client = new PaymentClient();
                var payment = await client.GetAsync(paymentId);
                
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener pago {paymentId} de MercadoPago");
                throw;
            }
        }

        public string GetPaymentStatusDescription(string status)
        {
            return status switch
            {
                "approved" => "Aprobado",
                "pending" => "Pendiente",
                "in_process" => "En proceso",
                "rejected" => "Rechazado",
                "cancelled" => "Cancelado",
                "refunded" => "Reembolsado",
                _ => "Desconocido"
            };
        }
    }
}
