using System.Threading.Tasks;
using ProselApp.Models;
using ProselApp.Repositories.Interfaces;
using ProselApp.Services.Interfaces;

namespace ProselApp.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository msgRepos;

        public MessageService(IMessageRepository msgRepos)
        {
            this.msgRepos = msgRepos;
        }

        public async Task AddAsync(Message message)
        {
            await msgRepos.AddAsync(message);
        }
    }
}