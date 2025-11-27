using System;
using Entity.Domain.Models.Implements.parameters;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.parametersDataInit
{
    public static class PaymentFrequencyDataInit
    {
        public static void SeedPaymentFrequency(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<PaymentFrequency>().HasData(
                // Frecuencias existentes
                new PaymentFrequency
                {
                    id = 1,
                    active = true,
                    is_deleted = false,
                    intervalPage = "MENSUAL",
                    IntervalType = "Months",
                    IntervalValue = 1,
                    created_date = seedDate
                },
                new PaymentFrequency
                {
                    id = 2,
                    active = true,
                    is_deleted = false,
                    intervalPage = "QUINCENAL",
                    IntervalType = "Days",
                    IntervalValue = 15,
                    created_date = seedDate
                },
                new PaymentFrequency
                {
                    id = 3,
                    active = true,
                    is_deleted = false,
                    intervalPage = "BIMESTRAL",
                    IntervalType = "Months",
                    IntervalValue = 2,
                    created_date = seedDate
                },

                // ✅ NUEVAS FRECUENCIAS (opcionales)
                new PaymentFrequency
                {
                    id = 4,
                    active = true,
                    is_deleted = false,
                    intervalPage = "SEMANAL",

                    IntervalType = "Days",
                    IntervalValue = 7,
                    created_date = seedDate
                },
                new PaymentFrequency
                {
                    id = 5,
                    active = true,
                    is_deleted = false,
                    intervalPage = "TRIMESTRAL",
                    IntervalType = "Months",
                    IntervalValue = 3,
                    created_date = seedDate
                },
                new PaymentFrequency
                {
                    id = 6,
                    active = true,
                    is_deleted = false,
                    intervalPage = "SEMESTRAL",
                    IntervalType = "Months",
                    IntervalValue = 6,
                    created_date = seedDate
                },
                new PaymentFrequency
                {
                    id = 7,
                    active = true,
                    is_deleted = false,
                    intervalPage = "ANUAL",
                    IntervalType = "Years",
                    IntervalValue = 1,
                    created_date = seedDate
                }
            );
        }
    }
}