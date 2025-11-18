
using Business.Interfaces.Notificacion;
using Entity.DTOs.Default.Notificacion;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    /// <summary>
    /// Implementación de <see cref="INotificationRealtimeService"/> basada en SignalR.
    /// Envía las notificaciones a los grupos <c>user-{{id}}</c> gestionados por <see cref="NotificationsHub"/>.
    /// </summary>
    public class SignalRNotificationRealtimeService : INotificationRealtimeService
    {
        private readonly IHubContext<NotificationsHub> _hub;

        public SignalRNotificationRealtimeService(IHubContext<NotificationsHub> hub)
        {
            _hub = hub;
        }

        public Task PushAsync(NotificationDto notification)
        {
            return _hub.Clients
                .Group($"user-{notification.RecipientUserId}")
                .SendAsync("notifications:new", notification);
        }
    }
}

