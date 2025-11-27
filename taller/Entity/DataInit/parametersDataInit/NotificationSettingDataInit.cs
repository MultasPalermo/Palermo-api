using Entity.Domain.Models.Implements.parameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DataInit.parametersDataInit
{
    public static class NotificationSettingDataInit
    {
        public static void SeedNotificationSetting(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationSetting>().HasData(
                new NotificationSetting
                {
                    id = 1,
                    Name = "Recordatorio 30 segundos",
                    Days = 30,
                    Description = "Primer recordatorio después de la infracción.",
                    TimeUnit = "SECONDS",
                    active = true
                },
                new NotificationSetting
                {
                    id = 2,
                    Name = "Recordatorio 60 segundos",
                    Days = 60,
                    Description = "Segundo recordatorio después de la infracción.",
                    TimeUnit = "SECONDS",
                    active = true
                },
                new NotificationSetting
                {
                    id = 3,
                    Name = "Recordatorio 80 segundos",
                    Days = 80,
                    Description = "Tercer recordatorio después de la infracción.",
                    TimeUnit = "SECONDS",
                    active = true
                },
                  new NotificationSetting
                  {
                      id = 4,
                      Name = "Recordatorio 100 segundos",
                      Days = 100,
                      Description = "cobrojuridico recordatorio después de la infracción",
                      TimeUnit = "SECONDS",
                      active = true
                  },
                new NotificationSetting
                {
                    id = 5,
                    Name = "Recordatorio 120 segundos",
                    Days = 120,
                    Description = "CobroCoactivo recordatorio después de la infracción.",
                    TimeUnit = "SECONDS",
                    active = true
                }
            );
        }
    }

}
