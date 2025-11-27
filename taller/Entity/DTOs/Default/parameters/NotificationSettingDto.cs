using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.parameters
{
    public class NotificationSettingDto : IHasId
    {
        public int id { get; set; }
        public string Name { get; set; } = null!;
        public int Days { get; set; }
        public string? Description { get; set; }

        public bool Active { get; set; }
        public bool is_deleted { get; set; }
        public DateTime created_date { get; set; }

        public string TimeUnit { get; set; } = "DAYS";
    }

}
