using Entity.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Select.parameters
{
    public class NotificationSettingSelect
    {
        public int id { get; set; }
        public string Name { get; set; } = null!;
        public int Days { get; set; }
        public string? Description { get; set; }
        public string TimeUnit { get; set; } = "DAYS";

        public bool Active { get; set; }
    }

}
