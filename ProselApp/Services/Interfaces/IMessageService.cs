using System.Collections.Generic;
using System.Threading.Tasks;
using ProselApp.Models;

namespace ProselApp.Services.Interfaces
{
    public interface IMessageService
    {
         Task AddAsync(Message message);
         Task DeleteAsync(Message message);
         Task DeleteMultipleAsync(List<Message> msgs);
         Task<List<Message>> GetAllAsync(string pesquisa);
         Task<Message> GetByCodeAsync(int id);
         Task<List<Message>> GetMultipleMsgsAsync(List<int> msgscode);
         Task LogicallyDeleteAsync(Message message);
         Task LogicallyDeleteMultipleAsync(List<Message> msgs);
         Task UpdateMsgAsync(Message msg);
         Task UpdateMultipleMsgsAsync(List<Message> msgs);
    }
}
