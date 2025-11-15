using Entity.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.parameters
{
    public class NotificationSetting : BaseModel
    {
        public string Name { get; set; } = null!;
        public int Days { get; set; }
        public string? Description { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string TimeUnit { get; set; } = "DAYS";
    }
}
