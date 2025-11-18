using System;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.EntitiesDataInit
{
    public static class UserInfractionDataInit
    {
        public static void SeedUserInfraction(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            // Simulación: activar coactivo 30 días después
            var coactiveDate = seedDate.AddDays(30);      // 2025-01-31
            var lastInterestDate = seedDate.AddDays(29);  // 2025-01-30

            modelBuilder.Entity<UserInfraction>().HasData(
                new UserInfraction
                {
                    id = 1,
                    UserId = 1,
                    InfractionId = 1,
                    UserNotificationId = 1,
                    dateInfraction = seedDate,
                    stateInfraction = EstadoMulta.Pendiente,
                    StatusCollection = EstadoCobro.CobroPrejuridico,
                    smldvValueAtCreation = 43500m,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                    paymentDue3Days = seedDate.AddDays(3).Date,
                    paymentDue15Days = seedDate.AddDays(15).Date,
                    paymentDue25Days = seedDate.AddDays(25).Date,

                    // 🔥 Nuevos campos para que el cálculo sí funcione
                    InitialAmount = 174000m,
                    IsCoactive = true,
                    CoactiveActivatedOn = coactiveDate,
                    LastInterestAppliedOn = lastInterestDate,
                    AccruedInterest = 0m,
                    DaysOfDelay = 0,
                    TotalToPay = 174000m
                },
                new UserInfraction
                {
                    id = 2,
                    UserId = 1,
                    InfractionId = 14,
                    UserNotificationId = 2,
                    dateInfraction = seedDate,
                    stateInfraction = EstadoMulta.Pendiente,
                    StatusCollection = EstadoCobro.CobroJuridico,
                    smldvValueAtCreation = 43500m,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                    paymentDue3Days = seedDate.AddDays(3).Date,
                    paymentDue15Days = seedDate.AddDays(15).Date,
                    paymentDue25Days = seedDate.AddDays(25).Date,

                    InitialAmount = 348000m,
                    IsCoactive = true,
                    CoactiveActivatedOn = coactiveDate,
                    LastInterestAppliedOn = lastInterestDate,
                    AccruedInterest = 0m,
                    DaysOfDelay = 0,
                    TotalToPay = 348000m
                },
                new UserInfraction
                {
                    id = 3,
                    UserId = 2,
                    InfractionId = 27,
                    UserNotificationId = 1,
                    dateInfraction = seedDate,
                    stateInfraction = EstadoMulta.Pendiente,
                    StatusCollection = EstadoCobro.CobroCoactivo,
                    smldvValueAtCreation = 43500m,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                    paymentDue3Days = seedDate.AddDays(3).Date,
                    paymentDue15Days = seedDate.AddDays(15).Date,
                    paymentDue25Days = seedDate.AddDays(25).Date,

                    InitialAmount = 696000m,
                    IsCoactive = true,
                    CoactiveActivatedOn = coactiveDate,
                    LastInterestAppliedOn = lastInterestDate,
                    AccruedInterest = 0m,
                    DaysOfDelay = 0,
                    TotalToPay = 696000m
                },
                new UserInfraction
                {
                    id = 4,
                    UserId = 2,
                    InfractionId = 40,
                    UserNotificationId = 2,
                    dateInfraction = seedDate,
                    stateInfraction = EstadoMulta.Pendiente,
                    StatusCollection = EstadoCobro.CobroPrejuridico,
                    smldvValueAtCreation = 43500m,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                    paymentDue3Days = seedDate.AddDays(3).Date,
                    paymentDue15Days = seedDate.AddDays(15).Date,
                    paymentDue25Days = seedDate.AddDays(25).Date,

                    InitialAmount = 1392000m,
                    IsCoactive = true,
                    CoactiveActivatedOn = coactiveDate,
                    LastInterestAppliedOn = lastInterestDate,
                    AccruedInterest = 0m,
                    DaysOfDelay = 0,
                    TotalToPay = 1392000m
                }
            );
        }
    }
}
