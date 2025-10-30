using Entity.DTOs.Default.ModelSecurityDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.ModelSecurity.RolFormPermission
{
    public class RolFormPermissionValidator : AbstractValidator<RolFormPermissionDto>
    {
        public RolFormPermissionValidator()
        {
            RuleFor(x => x.rolid)
                .GreaterThan(0).WithMessage("Debe proporcionar un ID de rol válido.");

            RuleFor(x => x.formid)
                .GreaterThan(0).WithMessage("Debe proporcionar un ID de formulario válido.");

            RuleFor(x => x.permissionid)
                .GreaterThan(0).WithMessage("Debe proporcionar un ID de permiso válido.");
        }
    }

}
