using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Enums.Notification
{
    public class NotificationEnums
    {
        public enum NotificationType
        {
            System = 1,
            InfractionCreated = 2,
            InfractionExpiring = 3,
            Reminder = 4
        }

        public enum NotificationPriority
        {
            Info = 1,
            Warning = 2,
            Critical = 3
        }

        public enum NotificationStatus
        {
            Unread = 1,
            Read = 2,
            Archived = 3
        }
    }
}
