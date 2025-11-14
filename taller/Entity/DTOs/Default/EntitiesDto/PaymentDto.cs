using Entity.Domain.Enums;

namespace Entity.DTOs.Default.EntitiesDto
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int UserInfractionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "COP";
        public long? MercadoPagoPaymentId { get; set; }
        public string? MercadoPagoStatus { get; set; }
        public string? PreferenceId { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? PayerEmail { get; set; }
        public string? PayerName { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
