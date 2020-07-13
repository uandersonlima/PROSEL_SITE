using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProselApp.Data;
using ProselApp.Models;
using ProselApp.Repositories.Interfaces;
using ProselApp.Services.Exceptions;

namespace ProselApp.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ProselAppContext context;

        public TokenRepository(ProselAppContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Token token)
        {
            await context.Token.AddAsync(token);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Token token)
        {
            context.Token.Remove(token);
            await context.SaveChangesAsync();
        }

        public async Task<Token> GetByHashAsync(string hash)
        {
            return await context.Token.Include(token => token.User).Where(token=> token.SecurityToken == hash).FirstOrDefaultAsync();
        }

        public async Task<Token> GetByIdAsync(int id)
        {
            return await context.Token.Where(tok => tok.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Token> GetLastTokenAsync()
        {
            return await context.Token.Where(tok => tok.User == null).OrderBy(tok => tok.Id).LastOrDefaultAsync();
        }
        public async Task<Token> GetTokenByUserAsync(User user)
        {
            return await context.Token.Where(tok => tok.User.Cpf == user.Cpf).FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(Token token)
        {
            try
            {
                context.Update(token);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}