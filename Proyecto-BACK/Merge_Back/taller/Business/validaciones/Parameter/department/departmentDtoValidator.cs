using Entity.DTOs.Interface.parameter;
using FluentValidation;

namespace Business.validaciones.Parameter.department
{
    public class DepartmentDtoValidator<T> : AbstractValidator<T> where T : Idepartment
    {
        public DepartmentDtoValidator()
        {

            // Nombre
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("El nombre del departamento es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

            // Código DANE
            RuleFor(x => x.daneCode)
                .GreaterThan(0)
                .WithMessage("El código DANE debe ser un número mayor que 0.");
        }
    }
}
