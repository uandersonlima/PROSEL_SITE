using System.Collections.Generic;
using System.Threading.Tasks;
using ProselApp.Models;

namespace ProselApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User entity);
        Task<bool> CheckEntityAsync(User entity);
        Task DeleteAsync(User entity);
        Task<User> GetByCpfAsync(string cpf);
        Task UpdateAsync(User entity);
        Task ChangePasswordAsync(User entity);
        List<User> GetUserByEmail(string email);
        Task<User> GetUserByLogin(string email, string senha);
        Task<int> NumberOfUserWithADM();
        int NumberOfUserWithoutADM();
        Task UpdateProfileAsync(User entity);
    }
}