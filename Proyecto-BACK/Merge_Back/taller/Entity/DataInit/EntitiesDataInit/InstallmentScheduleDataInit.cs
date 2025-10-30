using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit.EntitiesDataInit
{
    public static class InstallmentScheduleDataInit
    {
        public static void SeedInstallmentSchedule(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<InstallmentSchedule>().HasData(
                new InstallmentSchedule
                {
                    id = 1,
                    Number = 1,
                    PaymentDate = seedDate.AddMonths(2),
                    Amount = 32625,
                    RemainingBalance = 97900,
                    IsPaid = false,
                    PaymentAgreementId = 1,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new InstallmentSchedule
                {
                    id = 2,
                    Number = 2,
                    PaymentDate = seedDate.AddMonths(3),
                    Amount = 32625,
                    RemainingBalance = 65275,
                    IsPaid = false,
                    PaymentAgreementId = 1,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                }
            );
        }
    }
}
