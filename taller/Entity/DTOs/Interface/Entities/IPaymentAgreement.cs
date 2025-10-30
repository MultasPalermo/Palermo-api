using System;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Interface.Entities
{
    public interface IPaymentAgreement : IHasId
    {
        int id { get; set; }

        // Datos generales
        string address { get; set; }
        string neighborhood { get; set; }
        string AgreementDescription { get; set; }
        DateTime expeditionCedula { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }

        // Datos financieros
        decimal BaseAmount { get; set; }
        bool IsPaid { get; set; }

        // Relaciones
        int userInfractionId { get; set; }
        int paymentFrequencyId { get; set; }
        int typePaymentId { get; set; }

        // Plan de cuotas (opcional)
        int? Installments { get; set; }
        decimal? MonthlyFee { get; set; }
    }
}
