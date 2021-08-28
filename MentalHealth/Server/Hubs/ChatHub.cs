using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MentalHealth.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string chat)
        {
            await Clients.All.SendAsync("ReceiveMessage", chat);
        }
    }
}
