using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entity.Domain.Enums.Notification.NotificationEnums;

namespace Entity.DTOs.Default.Notificacion
{
    public class NotificationDto 
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
        public NotificationPriority Priority { get; set; }
        public NotificationStatus Status { get; set; }
        public int RecipientUserId { get; set; }
        public string? ActionRoute { get; set; }
        public DateTime created_date { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
