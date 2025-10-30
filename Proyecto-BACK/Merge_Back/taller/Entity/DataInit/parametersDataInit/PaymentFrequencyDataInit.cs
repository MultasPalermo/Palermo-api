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
                new PaymentFrequency { id = 1, active = true, is_deleted = false, intervalPage = "MENSUAL", dueDayOfMonth = 15, created_date = seedDate },
                new PaymentFrequency { id = 2, active = true, is_deleted = false, intervalPage = "QUINCENAL", dueDayOfMonth = 1, created_date = seedDate },
                new PaymentFrequency { id = 3, active = true, is_deleted = false, intervalPage = "BIMESTRAL", dueDayOfMonth = 10, created_date = seedDate }
            );
        }
    }
}
