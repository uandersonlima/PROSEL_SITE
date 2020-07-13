
using System.Threading.Tasks;
using ProselApp.Models;
using ProselApp.Repositories.Interfaces;

namespace ProselApp.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public Task AddAsync(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}