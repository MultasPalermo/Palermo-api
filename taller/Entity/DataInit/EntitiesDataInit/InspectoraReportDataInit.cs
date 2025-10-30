using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.EntitiesDataInit
{
    public static class InspectoraReportDataInit
    {
        public static void SeetInspectoraReportData(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<InspectoraReport>().HasData(
                new InspectoraReport
                {
                    id = 1,
                    total_fines = 2,
                    message = "se integra una nueva multa",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                },
                 new InspectoraReport
                 {
                     id = 2,
                     total_fines = 3,
                     message = "se integra una nueva multa",
                     active = true,
                     is_deleted = false,
                     created_date = seedDate
                 }
                );
        }
    }
}
