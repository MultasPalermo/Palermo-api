using Entity.Domain.Models.Implements.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.Entities.TypeInfraction
{
    public class TypeInfractionValidator : AbstractValidator<InfractionDto>
    {
        public TypeInfractionValidator()
        {
            RuleFor(x => x.type_Infraction)
                .NotEmpty().WithMessage("El tipo de infracción es obligatorio.")
                .MaximumLength(100).WithMessage("El tipo de infracción no debe superar los 100 caracteres.");


            RuleFor(x => x.description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no debe superar los 500 caracteres.");
        }
    }
}
