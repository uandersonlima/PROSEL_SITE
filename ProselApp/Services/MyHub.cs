using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ProselApp.Services
{
    public class MyHub : Hub
    {
        public async Task Notify(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}