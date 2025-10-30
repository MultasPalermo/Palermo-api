using Entity.DTOs.Default.LoginDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.Auth
{
    public class DocumentDtoValidator : AbstractValidator<DocumentLoginDto>
    {
        public DocumentDtoValidator()
        {
            RuleFor(x => x.DocumentTypeId)
              .NotNull().WithMessage("El tipo de documento es obligatorio.")
              .GreaterThan(0).WithMessage("El tipo de documento debe ser un entero mayor que 0.");

            RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("El número de documento es obligatorio.")
            .Matches(@"^\d+$").WithMessage("El número de documento solo debe contener números.")
            .Length(10).WithMessage("El número de documento debe tener exactamente 10 dígitos.");


            RuleFor(x => x.RecaptchaToken)
                .NotEmpty().WithMessage("El token de reCAPTCHA es obligatorio.");

            // Si quieres exigir que el action sea "documento":
            RuleFor(x => x.RecaptchaAction)
                .Must(a => string.IsNullOrWhiteSpace(a) || a == "documento")
                .WithMessage("La acción de reCAPTCHA no es válida.");
        }
    }
}
