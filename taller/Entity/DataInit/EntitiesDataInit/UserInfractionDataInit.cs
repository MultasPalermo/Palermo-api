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
                }
            );
        }

    }
}
