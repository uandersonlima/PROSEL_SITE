using System;
using System.Threading.Tasks;
using ProselApp.Models;

namespace ProselApp.Services.Interfaces
{
    public interface ITokenService
    {
        Task AddAsync(Token token);
        Task<bool> CheckTokenAsync(Token token);
        Task CreateNewKeyAsync();
        Task DeleteAsync(int id);
        Task DeleteAsync(Token token);
        Task<TimeSpan> ElapsedTimeLastTokenAsync();
        Task<Token> GetByHashAsync(string hash);
        Task<Token> GetByIdAsync(int id);
        Task<Token> GetLastTokenAsync();
        Task<Token> GetTokenByUserAsync(User user);
        Task<bool> TokenIsNotExpiredAsync(User user);
        Task UpdateAsync(Token token);

    }
}