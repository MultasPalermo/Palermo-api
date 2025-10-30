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
    public static class UserNotificationDataInit
    {
        public static void SeedUserNotificacion(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<UserNotification>().HasData(
                 new UserNotification
                 {
                     id = 1,
                     message = "tienes una infraccion por favor acercate antes del 12 de marzo para sucdazanar tu multa o podria iniciar un cobro coativo luego del plazo",
                     shippingDate = seedDate,
                     active = true,
                     is_deleted = false,
                     created_date = seedDate
                 },
                new UserNotification
                {
                    id = 2,
                    message = "tienes una infraccion por favor acercate antes del 12 de julio para sucdazanar tu multa o podria iniciar un cobro coativo luego del plazo",
                    shippingDate = seedDate,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                }
                );
        }
    }
}
