using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ProselApp.Services
{
    public class NotificationService : Hub
    {
        public async Task Notify()
        {
           await Clients.All.SendAsync("novamsg");
        }
    }
}