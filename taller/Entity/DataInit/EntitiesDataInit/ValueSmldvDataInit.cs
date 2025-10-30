using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.EntitiesDataInit
{
    public static class ValueSmldvDataInit
    {
        public static void SeedValueSmldv(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<ValueSmldv>().HasData(
                 new ValueSmldv
                 {
                     id = 1,
                     value_smldv = 43500m,
                     Current_Year = 2024,
                     minimunWage = 1425000m,
                     active = true,
                     is_deleted = false,
                     created_date  = seedDate,
                 }
                //new ValueSmldv
                //{
                //    id = 2,
                //    value_smldv = 43500m,
                //    Current_Year = 2022,
                //    minimunWage = 1100000m,
                //    active = true,
                //    is_deleted = false,
                //    created_date = seedDate,
                //}
                );
        }
    }
}   
