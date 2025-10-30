using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Interface.Entities;
using FluentValidation;

public class InspectoraReportValidator<T> : AbstractValidator<T>
    where T : IInspectoraReport
{
    public InspectoraReportValidator()
    {
        RuleFor(x => x.report_date)
            .NotEmpty().WithMessage("La fecha del reporte es obligatoria.");

        RuleFor(x => x.total_fines)
            .GreaterThanOrEqualTo(0).WithMessage("El total de multas no puede ser negativo.");

        RuleFor(x => x.message)
            .NotEmpty().WithMessage("El mensaje no puede estar vacío.")
            .MaximumLength(500).WithMessage("El mensaje no debe superar 500 caracteres.");
    }
}


