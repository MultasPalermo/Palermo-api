using System;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.EntitiesDataInit
{
    public static class TypePaymentDataInit
    {
        public static void SeedTypePayment(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<TypePayment>().HasData(
                new TypePayment
                {
                    id = 1,
                    name = "efectivo",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new TypePayment
                {
                    id = 2,
                    name = "nequi",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new TypePayment
                {
                    id = 3,
                    name = "tarjeta crédito",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new TypePayment
                {
                    id = 4,
                    name = "tarjeta débito",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new TypePayment
                {
                    id = 5,
                    name = "daviplata",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                }
            );
        }
    }
}
