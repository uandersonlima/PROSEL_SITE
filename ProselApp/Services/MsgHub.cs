using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ProselApp.Services
{
    public class MsgHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}