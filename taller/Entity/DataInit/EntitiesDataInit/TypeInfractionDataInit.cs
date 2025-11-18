using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit.EntitiesDataInit
{
    public static class TypeInfractionDataInit
    {
        public static void SeedTypeInfraction(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<TypeInfraction>().HasData(
                new TypeInfraction
                {
                    id = 1,
                    Name = "Multa Tipo uno",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                },
                new TypeInfraction
                {
                    id = 2,
                    Name = "Multa Tipo dos",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new TypeInfraction
                {
                    id = 3,
                    Name = "Multa Tipo tres",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                },
                new TypeInfraction
                {
                    id = 4,
                    Name = "Multa Tipo cuatro",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                }
            );
        }
    }
}
