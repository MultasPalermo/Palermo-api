using Entity.DTOs.Default.AnexarMulta;
using FluentValidation;

namespace Business.Validaciones.Entities.UserInfraction
{
    public class CreateInfractionRequestValidator : AbstractValidator<CreateInfractionRequestDto>
    {
        public CreateInfractionRequestValidator()
        {
            // 🔹 Nombre
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.")
                .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

            // 🔹 Apellido
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.")
                .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("El apellido solo puede contener letras y espacios.");

            // 🔹 Tipo de documento
            RuleFor(x => x.DocumentTypeId)
                .GreaterThan(0).WithMessage("Debe seleccionar un tipo de documento válido.");

            // 🔹 Número de documento (dependiendo del tipo)
            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El número de documento es obligatorio.")
                .Matches(@"^\d+$").WithMessage("El número de documento debe contener solo números.")
                .MinimumLength(6).WithMessage("El número de documento debe tener mínimo 6 dígitos.")
                .MaximumLength(15).WithMessage("El número de documento no puede superar los 15 dígitos.");

            // Reglas adicionales según el tipo de documento (ejemplo para Colombia)
            When(x => x.DocumentTypeId == 1, () => // CC
            {
                RuleFor(x => x.DocumentNumber)
                    .Length(8, 10).WithMessage("La cédula de ciudadanía debe tener entre 8 y 10 dígitos.");
            });

            When(x => x.DocumentTypeId == 2, () => // CE
            {
                RuleFor(x => x.DocumentNumber)
                    .Length(6, 15).WithMessage("La cédula de extranjería debe tener entre 6 y 15 dígitos.");
            });

            When(x => x.DocumentTypeId == 3, () => // NIT
            {
                RuleFor(x => x.DocumentNumber)
                    .Matches(@"^\d{9,10}-\d$").WithMessage("El NIT debe tener 9 o 10 dígitos más un dígito de verificación (ej: 900123456-7).");
            });

            When(x => x.DocumentTypeId == 4, () => // Pasaporte
            {
                RuleFor(x => x.DocumentNumber)
                    .Matches(@"^[A-Z0-9]{6,15}$").WithMessage("El pasaporte debe contener entre 6 y 15 caracteres alfanuméricos.");
            });

            // 🔹 Tipo de infracción
            RuleFor(x => x.TypeInfractionId)
                .GreaterThan(0).WithMessage("Debe seleccionar un tipo de infracción válido.");

        }
    }
}
