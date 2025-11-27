
using Entity.DTOs.Default.Notificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Notificacion
{
    /// <summary>
    /// Abstracción para enviar notificaciones en tiempo real a los clientes.
    /// Permite desacoplar la capa de negocio de la infraestructura SignalR.
    /// </summary>
    public interface INotificationRealtimeService
    {
        Task PushAsync(NotificationDto notification);
    }
}
