
using System.Threading.Tasks;
using ProselApp.Data;
using ProselApp.Models;
using ProselApp.Repositories.Interfaces;

namespace ProselApp.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ProselAppContext context;

        public MessageRepository(ProselAppContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Message message)
        {
            await context.Message.AddAsync(message);
            await context.SaveChangesAsync();
        }
    }
}