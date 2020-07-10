using System.Threading.Tasks;
using ProselApp.Models.AcessCode;

namespace ProselApp.Repositories.Interfaces
{
    public interface ICodeRepository
    {
        Task AddAsync(AccessCode accessCode);
        Task DeleteAsync(AccessCode accessCode);
        Task<AccessCode> SearchCodeAsync(AccessCode accessCode);
        Task<AccessCode> SearchAndValidateCodeAsync(AccessCode accessCode);
    }
}