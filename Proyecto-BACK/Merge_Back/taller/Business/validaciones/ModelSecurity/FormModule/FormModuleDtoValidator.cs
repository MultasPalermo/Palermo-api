using Entity.DTOs.Interface.ModelSecurity;
using FluentValidation;

namespace Business.Validaciones.ModelSecurity.FormModule
{
    public class FormModuleDtoValidator<T> : AbstractValidator<T> where T : IFormModule
    {
        public FormModuleDtoValidator()
        {

            // formid válido
            RuleFor(x => x.formid)
                .GreaterThan(0)
                .WithMessage("El formulario es obligatorio y debe ser mayor a 0.");

            // moduleid válido
            RuleFor(x => x.moduleid)
                .GreaterThan(0)
                .WithMessage("El módulo es obligatorio y debe ser mayor a 0.");
        }
    }
}
