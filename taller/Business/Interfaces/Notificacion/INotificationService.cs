using Business.Interfaces.BusinessBasic;
using Entity.DTOs.Default.Notificacion;
using static Entity.Domain.Enums.Notification.NotificationEnums;

namespace Business.Interfaces.Notificacion
{
    public interface INotificationService
        : IBusiness<NotificationCreateDto, NotificationDto>
    {
        Task<IReadOnlyList<NotificationDto>> GetFeedAsync(int userId, NotificationStatus? status = null, int take = 20);
        Task<IReadOnlyList<NotificationDto>> GetUnreadAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId, int userId);
        Task<int> MarkAllAsReadAsync(int userId);
    }
}
