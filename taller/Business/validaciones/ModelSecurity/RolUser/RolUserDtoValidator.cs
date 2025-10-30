using Entity.DTOs.Default.ModelSecurityDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.ModelSecurity.RolUser
{
    public class RolUserValidator : AbstractValidator<RolUserDto>
    {
        public RolUserValidator()
        {
            RuleFor(x => x.userId)
                .GreaterThan(0).WithMessage("Debe proporcionar un ID de usuario válido.");

            RuleFor(x => x.rolId)
                .GreaterThan(0).WithMessage("Debe proporcionar un ID de rol válido.");
        }
    }

}
