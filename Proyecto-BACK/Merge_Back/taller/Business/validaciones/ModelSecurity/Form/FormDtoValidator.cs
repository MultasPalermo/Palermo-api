using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Interface.ModelSecurity;
using FluentValidation;

namespace Business.validaciones.ModelSecurity.Form
{
    public class FormDtoValidator<T> : AbstractValidator<T> where T : IForm
    {
        public FormDtoValidator()
        {

            // name obligatorio y con longitud máxima
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

            // description obligatorio y con longitud máxima
            RuleFor(x => x.description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.")
                 .Matches(@"^[a-zA-Z\s]+$").WithMessage("La descripsion solo puede contener letras y espacios.");
        }
    }
}
