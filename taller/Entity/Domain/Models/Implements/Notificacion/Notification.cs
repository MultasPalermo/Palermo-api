using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.ModelSecurity;
using static Entity.Domain.Enums.Notification.NotificationEnums;

namespace Entity.Domain.Models.Implements.Notificacion
{
    public class Notification : BaseModel
    {
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; } = NotificationType.System;
        public NotificationPriority Priority { get; set; } = NotificationPriority.Info;
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;

        public int RecipientUserId { get; set; }
        public User? RecipientUser { get; set; }

        public string? ActionRoute { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
