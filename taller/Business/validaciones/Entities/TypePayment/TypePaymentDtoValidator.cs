using Entity.DTOs.Interface.Entities;
using FluentValidation;

namespace Business.validaciones.Entities.TypePayment
{
    public class TypePaymentDtoValidator<T> : AbstractValidator<T> where T : ITypePayment
    {
        public TypePaymentDtoValidator()
        {

            // Nombre
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.")
                .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage("El nombre contiene caracteres inválidos.");

        }
    }
}
