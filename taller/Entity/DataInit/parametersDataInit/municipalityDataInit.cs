using System;
using Entity.Domain.Models.Implements.parameters;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.dataInitParameters
{
    public static class MunicipalityDataInit
    {
        public static void seedMunicipality(this ModelBuilder modelBuilder)
        {

            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<municipality>().HasData(
             new municipality { id = 1, name = "Medellín", daneCode = 5001, departmentId = 1, active = true, is_deleted = false, created_date =seedDate },
             new municipality { id = 2, name = "Cali", daneCode = 76001, departmentId = 3, active = true, is_deleted = false, created_date =seedDate },
             new municipality { id = 3, name = "Bogotá", daneCode = 11001, departmentId = 4, active = true, is_deleted = false, created_date =seedDate },
             new municipality { id = 4, name = "Soacha", daneCode = 25754, departmentId = 2, active = true, is_deleted = false, created_date =seedDate }
         );
        }
    }
}
