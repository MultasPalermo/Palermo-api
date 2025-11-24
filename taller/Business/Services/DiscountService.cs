using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using System;

namespace Business.Services
{
    // Nota: Asumo que FineCalculationDetailDto está definido fuera de esta clase 
    // y tiene las propiedades que estás usando (formula, percentaje, etc.).
    // También asumo que UserInfractionDto tiene la propiedad typeInfractionId y smldvValueAtCreation.

    public class DiscountService
    {
        public FineCalculationDetailDto Calculate(
             UserInfractionDto infraction,
             decimal baseAmount,
             int smldvId,
             string smldvName,
             string typeInfractionName,
             EstadoCobro statusCollection) // 👈 El estado de Cobro determina el descuento
        {
            // 1. Determinar el porcentaje de descuento basado en el estado.
            // REGLA: 50% de descuento solo si el estado es CobroPrejuridico.
            decimal porcentaje = statusCollection switch
            {
                EstadoCobro.CobroPrejuridico => 0.50m, // 50% de descuento

                // 0% de descuento en el primer recordatorio y todos los estados posteriores.
                EstadoCobro.prejuridico3Dias or
                EstadoCobro.prejuridico15Dias or
                EstadoCobro.prejuridico25Dias or
                EstadoCobro.CobroJuridico or
                EstadoCobro.CobroCoactivo => 0.00m,

                _ => 0.00m // Por defecto
            };

            // 2. Calcular el monto del descuento y el total final
            decimal discount = baseAmount * porcentaje;
            decimal totalCalculation = baseAmount - discount;

            // 3. Devolver el DTO
            return new FineCalculationDetailDto
            {
                formula = $"Base {baseAmount:C} - {porcentaje * 100}% de descuento ({discount:C}) [Estado: {statusCollection}]",
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