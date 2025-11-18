using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.Notificacion;
using static Entity.Domain.Enums.Notification.NotificationEnums;

namespace Data.Interfaces.IDataImplement.Notificacion
{
    public interface INotificationRepository : IData<Notification>
    {
        Task<IReadOnlyList<Notification>> GetUnreadByUserAsync(int userId);
        Task<IReadOnlyList<Notification>> GetByUserAsync(int userId, NotificationStatus? status = null, int take = 20);
        Task<int> MarkAllAsReadAsync(int userId);

        Task<bool> HasRecentNotificationAsync(int recipientUserId, NotificationType type, string? actionRoute, DateTime since);
    }
}
