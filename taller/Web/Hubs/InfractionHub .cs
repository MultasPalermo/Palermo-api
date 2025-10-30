using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class InfractionHub : Hub
    {
        // Método de prueba (opcional)
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
