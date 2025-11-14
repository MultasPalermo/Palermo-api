namespace Entity.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 0,        // Pendiente
        Approved = 1,       // Aprobado
        InProcess = 2,      // En proceso
        Rejected = 3,       // Rechazado
        Cancelled = 4,      // Cancelado
        Refunded = 5        // Reembolsado
    }
}
