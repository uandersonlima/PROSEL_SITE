using System.Threading.Tasks;
using ProselApp.Models;

namespace ProselApp.Repositories.Interfaces
{
    public interface IMessageRepository
    {
         Task AddAsync(Message message);
    }
}