using Entity.Domain.Models.Implements.Entities;
using System;

namespace Business.Services
{
    public class DiscountService
    {
        /// <summary>
        /// Calcula el detalle de descuento de una infracción según los días transcurridos.
        /// </summary>
        /// <param name="infraction">DTO de la infracción</param>
        /// <param name="baseAmount">Monto base calculado según SMLDV</param>
        /// <param name="smldvId">Id del SMLDV vigente</param>
        /// <param name="smldvName">Nombre o descripción del SMLDV</param>
        /// <param name="typeInfractionName">Nombre real del tipo de infracción</param>
        /// <returns>DTO con detalle del cálculo</returns>
        public FineCalculationDetailDto Calculate(
          UserInfractionDto infraction,
          decimal baseAmount,
          int smldvId,
          string smldvName,
          string typeInfractionName)
        {
            int daysPassed = (DateTime.Now.Date - infraction.dateInfraction.Date).Days;

            // Regla: 50% solo durante los primeros 5 días
            decimal porcentaje = daysPassed <= 5 ? 0.5m : 0m;

            decimal discount = baseAmount * porcentaje;
            decimal totalCalculation = baseAmount - discount;

            return new FineCalculationDetailDto
            {
                formula = $"Base {baseAmount:C} - {porcentaje * 100}% de descuento ({discount:C})",
                percentaje = porcentaje,
                totalCalculation = totalCalculation,
                typeInfractionId = infraction.typeInfractionId,
                type_Infraction = typeInfractionName,
                valueSmldvId = smldvId,
                valueSmldvName = smldvName,
                SmldvValueAtCreation = infraction.smldvValueAtCreation
            };
        }

    }
}
