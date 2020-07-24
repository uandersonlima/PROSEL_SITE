using System.Collections.Generic;
using System.Threading.Tasks;
using ProselApp.Models;

namespace ProselApp.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        Task AddAsync(Token token);
        Task DeleteAsync(Token token);
        Task<List<Token>> GetAllTokens();
        Task<Token> GetByHashAsync(string hash);
        Task<Token> GetByIdAsync(int id);
        Task<Token> GetLastTokenAsync();
        Task<Token> GetTokenByUserAsync(User user);
        Task UpdateAsync(Token token);
    }
}