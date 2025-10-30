using Entity.Domain.Models.Implements.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.Entities.ValueSmldv
{
    public class ValueSmldvValidator : AbstractValidator<ValueSmldvDto>
    {
        public ValueSmldvValidator()
        {
            RuleFor(x => x.value_smldv)
                .GreaterThan(0).WithMessage("El valor SMMLV debe ser mayor que cero.");

            RuleFor(x => x.Current_Year)
                .GreaterThan(1900).WithMessage("El año debe ser válido.")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage("El año no puede ser superior al próximo año.");

            RuleFor(x => x.minimunWage)
                .GreaterThan(0).WithMessage("El salario mínimo debe ser mayor que cero.");
        }
    }

}
