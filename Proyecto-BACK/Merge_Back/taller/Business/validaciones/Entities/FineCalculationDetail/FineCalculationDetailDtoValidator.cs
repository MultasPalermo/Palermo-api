using Entity.Domain.Models.Implements.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.validaciones.Entities.FineCalculationDetail
{
    public class FineCalculationDetailValidator : AbstractValidator<FineCalculationDetailDto>
    {
        public FineCalculationDetailValidator()
        {
            RuleFor(x => x.formula)
                .NotEmpty().WithMessage("La fórmula es obligatoria.")
                .MaximumLength(200).WithMessage("La fórmula no debe exceder los 200 caracteres.");

            //RuleFor(x => x.percentaje)
            //    .InclusiveBetween(0, 100)
            //    .WithMessage("El porcentaje debe estar entre 0% y 100%.");

            RuleFor(x => x.totalCalculation)
                .GreaterThanOrEqualTo(0).WithMessage("El cálculo total no puede ser negativo.");

            RuleFor(x => x.valueSmldvId)
                .GreaterThan(0).WithMessage("Debe seleccionar un valor de SMLDV válido.");

            RuleFor(x => x.typeInfractionId)
                .GreaterThan(0).WithMessage("Debe seleccionar un tipo de infracción válido.");
        }
    }

}
