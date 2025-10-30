using Entity.Domain.Models.Implements.Entities;
using FluentValidation;

namespace Business.validaciones.Entities.PaymentAgreement
{
    public class PaymentAgreementUpdateValidator : AbstractValidator<PaymentAgreementDto>
    {
        public PaymentAgreementUpdateValidator()
        {
            // 👇 Aquí solo validas lo que sí debe cumplirse en actualización

            RuleFor(x => x.Installments)
                .GreaterThan(0).WithMessage("El número de cuotas debe ser mayor que 0.");

            RuleFor(x => x.MonthlyFee)
                .GreaterThan(0).WithMessage("La cuota mensual debe ser mayor que 0.");
        }
    }
}
