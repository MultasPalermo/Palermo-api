using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.Payments;
using Entity.DTOs.Select.Entities;
using Entity.DTOs.Select.EntitiesSelectDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.IBusinessImplements.Entities
{
    public interface IMercadoPagoService
    {
        // 1) MULTA directa
        Task<MercadoPagoPreferenceResult> CreateCheckoutForInfractionAsync(
            UserInfractionSelectDto multa,
            UserSelectDto usuario);

        // 2) CUOTA de ACUERDO DE PAGO
        Task<MercadoPagoPreferenceResult> CreateCheckoutForAgreementAsync(
            InstallmentScheduleSelectDto cuota,
            PaymentAgreementSelectDto acuerdo,
            UserSelectDto usuario);

        // 3) Obtener pago
        Task<MercadoPagoPaymentInfoDto?> GetPaymentAsync(string paymentId);

        // 4) Validar webhook
        bool ValidateWebhookSignature(string? signature, string payload);

        // 5) Secret del webhook
        string WebhookSecret { get; }
    }

}
