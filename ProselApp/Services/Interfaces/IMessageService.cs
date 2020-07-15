using System.Threading.Tasks;
using ProselApp.Models;

namespace ProselApp.Services.Interfaces
{
    public interface IMessageService
    {
         Task AddAsync(Message message);
    }
}
