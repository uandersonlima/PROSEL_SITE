using System.Collections.Generic;
using System.Threading.Tasks;
using ProselApp.Models;
using ProselApp.Models.AcessCode;

namespace ProselApp.Services.Interfaces
{
    public interface IUserService
    {

        Task<bool> ActiveAccountAsync(User entity, AccessCode accessCode);
        Task AddAsync(User entity);
        Task ChangePasswordAsync(User entity);
        Task<bool> ChangePasswordByCodeAsync(User entity, AccessCode accessCode);
        Task<bool> CheckEntityAsync(User entity);
        Task DeleteAsync(string cpf);
        Task DeleteAsync(User entity);
        Task EnableOrDisableAsync(User entity);
        Task<User> GetByCpfAsync(string cpf);
        List<User> GetUserByEmail(string email);
        Task<User> GetUserByLogin(string email, string senha);
        Task<int> NumberOfUserWithADM();
        int NumberOfUserWithoutADM();
        Task UpdateAsync(User entity);
        Task UpdateProfileAsync(User entity);
    }
}