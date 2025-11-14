using Entity.Domain.Enums;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class Payment : BaseModel
    {
        public int UserInfractionId { get; set; }
        public UserInfraction UserInfraction { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "COP";
        
        // Información de MercadoPago
        public long? MercadoPagoPaymentId { get; set; }
        public string? MercadoPagoStatus { get; set; }
        public string? MercadoPagoStatusDetail { get; set; }
        public string? PreferenceId { get; set; }
        
        // Estado del pago
        public PaymentStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }
        
        // Información del pagador
        public string? PayerEmail { get; set; }
        public string? PayerName { get; set; }
        public string? PayerIdentification { get; set; }
        
        // Metadatos
        public string? PaymentMethod { get; set; }
        public string? TransactionDetails { get; set; }
    }
}
