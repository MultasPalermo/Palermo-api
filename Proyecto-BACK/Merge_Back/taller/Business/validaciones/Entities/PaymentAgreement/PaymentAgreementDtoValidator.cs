using Entity.DTOs.Interface.Entities;
using FluentValidation;

namespace Business.validaciones.Entities.PaymentAgreement
{
    public class PaymentAgreementDtoValidator<T> : AbstractValidator<T> where T : IPaymentAgreement
    {
        public PaymentAgreementDtoValidator()
        {
            // Dirección (Address)
            RuleFor(x => x.address)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .Matches(@"^[\p{L}0-9\s\.\,\-\#°/]+$").WithMessage("La dirección contiene caracteres inválidos.");

            // Barrio (Neighborhood)
            RuleFor(x => x.neighborhood)
                .NotEmpty().WithMessage("El barrio es obligatorio.")
                .Matches(@"^[\p{L}\s]+$").WithMessage("El barrio solo puede contener letras y espacios.");

            // Descripción del acuerdo
            RuleFor(x => x.AgreementDescription)
                .NotEmpty().WithMessage("La descripción del acuerdo es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.")
                .Matches(@"^[\p{L}0-9\s\.\,\-\#°/]+$").WithMessage("La descripción contiene caracteres inválidos.");

            // Cédula
            // Fecha de expedición de cédula
            RuleFor(x => x.expeditionCedula)
                .NotEmpty().WithMessage("La fecha de expedición de la cédula es obligatoria.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de expedición no puede ser futura.");


            // Teléfono
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("El número de teléfono es obligatorio.")
                .Matches(@"^[0-9]{7,10}$").WithMessage("El teléfono debe tener entre 7 y 10 dígitos.");

            // Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");

            // Monto base
            //RuleFor(x => x.BaseAmount)
            //    .GreaterThan(0).WithMessage("El monto base debe ser mayor que 0.");

            // Estado de pago (se valida que siempre tenga valor)
            RuleFor(x => x.IsPaid)
                .NotNull().WithMessage("El campo IsPaid es obligatorio.");

            // UserInfractionId
            RuleFor(x => x.userInfractionId)
                .GreaterThan(0).WithMessage("El campo UserInfractionId debe ser un número mayor que 0.");

            // PaymentFrequencyId
            RuleFor(x => x.paymentFrequencyId)
                .GreaterThan(0).WithMessage("El campo PaymentFrequencyId debe ser un número mayor que 0.");

            // TypePaymentId
            RuleFor(x => x.typePaymentId)
                .GreaterThan(0).WithMessage("El campo TypePaymentId debe ser un número mayor que 0.");

            // Número de cuotas (si se envía)
            RuleFor(x => x.Installments)
                .GreaterThan(0).When(x => x.Installments.HasValue)
                .WithMessage("El número de cuotas debe ser mayor que 0.");

            // Cuota mensual (si se envía)
            RuleFor(x => x.MonthlyFee)
                .GreaterThan(0).When(x => x.MonthlyFee.HasValue)
                .WithMessage("La cuota mensual debe ser mayor que 0.");

            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    if (dto.Installments.HasValue && dto.MonthlyFee.HasValue)
                    {
                        var total = dto.Installments.Value * dto.MonthlyFee.Value;

                        if (total != dto.BaseAmount)
                        {
                            context.AddFailure("Installments",
                                $"El total de cuotas ({dto.Installments} x {dto.MonthlyFee} = {total}) no coincide con el monto ({dto.BaseAmount}).");
                        }
                    }
                });

        }
    }
}
