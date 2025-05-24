using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SweetNela.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Enviar mensaje a todos los clientes conectados
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
