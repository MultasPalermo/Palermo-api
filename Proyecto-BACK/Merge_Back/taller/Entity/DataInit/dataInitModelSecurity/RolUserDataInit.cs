using System;
using Microsoft.EntityFrameworkCore;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace Entity.DataInit.dataInitModelSecurity
{
    public static class RolUserDataInit
    {
        /// <summary>
        /// Método de extensión para agregar datos semilla para la entidad RolUser.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo EF Core.</param>
        public static void SeedRolUser(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<RolUser>().HasData(
                new RolUser
                {
                    id = 1,
                    RolId = 1,   
                    UserId = 1,   
                    is_deleted = false,
                    created_date = seedDate,
                },
                new RolUser
                {
                    id = 2,
                    RolId = 2,
                    UserId = 2,
                    is_deleted = false,
                    created_date = seedDate
                }
            );
        }
    }
}
