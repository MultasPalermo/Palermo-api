using Entity.DTOs.Default.ModelSecurityDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.ModelSecurity.Rol
{
    public class RolValidator : AbstractValidator<RolDto>
    {
        public RolValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del rol no debe superar los 100 caracteres.");
        }
    }

}
