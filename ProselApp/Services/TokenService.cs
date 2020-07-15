using System;
using System.Threading.Tasks;
using ProselApp.Libraries.Text;
using ProselApp.Models;
using ProselApp.Repositories.Interfaces;
using ProselApp.Services.Interfaces;

namespace ProselApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly IEmailService emailSvc;
        private readonly ITokenRepository tokenRepos;

        public TokenService(IEmailService emailSvc, ITokenRepository tokenRepos)
        {
            this.emailSvc = emailSvc;
            this.tokenRepos = tokenRepos;
        }

        public async Task AddAsync(Token token)
        {
            await tokenRepos.AddAsync(token);
        }

        public async Task<bool> CheckTokenAsync(Token token)
        {
            var securitytoken = await GetByHashAsync(token.SecurityToken);
            if (securitytoken == null || securitytoken.User != null || DateTime.Now >= securitytoken.TokenExpiration)
            {
                return false;
            }
            return true;
        }
        public async Task CreateNewKeyAsync()
        {
            var securitytoken = GenKey.GetUniqueKey(16);

            Token token = new Token
            {
                SecurityToken = securitytoken,
                CreationDate = DateTime.Now,
                TokenExpiration = DateTime.Now.AddMonths(3)
            };
            await emailSvc.SendTokenToOwnerAsync(token);

            await AddAsync(token);
        }
        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(await GetByIdAsync(id));
        }
        public async Task DeleteAsync(Token token)
        {
            await tokenRepos.DeleteAsync(token);
        }
        public async Task<TimeSpan> ElapsedTimeLastTokenAsync()
        {
            var previousToken = await GetLastTokenAsync();
            if (previousToken != null)
            {
                return DateTime.Now.Subtract(previousToken.CreationDate);
            }
            return new TimeSpan(23, 59, 59);
        }

        public async Task<Token> GetByHashAsync(string hash)
        {
            return await tokenRepos.GetByHashAsync(hash);
        }

        public async Task<Token> GetByIdAsync(int id)
        {
            return await tokenRepos.GetByIdAsync(id);
        }

        public async Task<Token> GetLastTokenAsync()
        {
            return await tokenRepos.GetLastTokenAsync();
        }

        public async Task<Token> GetTokenByUserAsync(User user)
        {
            return await tokenRepos.GetTokenByUserAsync(user);
        }

        public async Task<bool> TokenIsExpiredAsync(User user)
        {
            var token = await GetTokenByUserAsync(user);
            return DateTime.Now > token.TokenExpiration;
        }

        public async Task UpdateAsync(Token token)
        {
            await tokenRepos.UpdateAsync(token);
        }
    }
}
