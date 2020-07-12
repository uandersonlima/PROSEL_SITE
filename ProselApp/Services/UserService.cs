using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProselApp.Models;
using ProselApp.Models.AcessCode;
using ProselApp.Models.Const;
using ProselApp.Repositories.Interfaces;
using ProselApp.Services.Exceptions;
using ProselApp.Services.Interfaces;

namespace ProselApp.Services
{
    public class UserService : IUserService
    {
        private readonly ICodeService codeSvc;
        private readonly IConfiguration conf;
        private readonly IUserRepository userRepos;
        public UserService(IUserRepository userRepos, ICodeService codeSvc, IConfiguration conf)
        {
            this.conf = conf;
            this.codeSvc = codeSvc; 
            this.userRepos = userRepos;
        }

        public async Task<bool> ActiveAccountAsync(User entity, AccessCode accessCode)
        {
            var receivedCode = await codeSvc.SearchAndValidateCodeAsync(accessCode);
            if (!(receivedCode is null))
            {
                await EnableOrDisableAsync(entity);
                await UpdateAsync(entity);
                await codeSvc.DeleteAsync(receivedCode);
                return true;
            }
            return false;
        }
        public async Task AddAsync(User entity)
        {
            if (entity.Email.ToLower() == conf.GetValue<string>("Email:Username").ToLower())
            {
                entity.AccessType = AccessType.Administrator;
                entity.AccountStatus = true;
            }
            else
            {
                entity.AccessType = AccessType.User;
                entity.AccountStatus = false;
            }
            await userRepos.AddAsync(entity);
        }

        public async Task ChangePasswordAsync(User entity)
        {
            await userRepos.ChangePasswordAsync(entity);
        }

        public async Task<bool> ChangePasswordByCodeAsync(User entity, AccessCode accessCode)
        {
            var receivedCode = await codeSvc.SearchAndValidateCodeAsync(accessCode);
            if (!(receivedCode is null))
            {
                if (!await CheckEntityAsync(entity))
                    throw new NotFoundException("User não existe");
                await ChangePasswordAsync(entity);
                await codeSvc.DeleteAsync(accessCode);
                return true;
            }
            return false;
        }

        public async Task<bool> CheckEntityAsync(User entity)
        {
            return await userRepos.CheckEntityAsync(entity);
        }

        public async Task DeleteAsync(string cpf)
        {
            await DeleteAsync(await GetByCpfAsync(cpf));
        }

        public async Task DeleteAsync(User entity)
        {
            await userRepos.DeleteAsync(entity);
        }

        public async Task EnableOrDisableAsync(User entity)
        {
            entity.AccountStatus = !entity.AccountStatus;
            await UpdateAsync(entity);
        }

        public async Task<List<User>> GetByPermission(string perm)
        {
            return await userRepos.GetByPermission(perm);
        }
        public async Task<User> GetByCpfAsync(string Cpf)
        {
            return await userRepos.GetByCpfAsync(Cpf);
        }
        //nao implementa async nesse
        public List<User> GetUserByEmail(string email)
        {
            return userRepos.GetUserByEmail(email);
        }

        public async Task<User> GetUserByLogin(string email, string senha)
        {
            return await userRepos.GetUserByLogin(email, senha);
        }

        public async Task<int> NumberOfUserWithADM()
        {
            return await userRepos.NumberOfUserWithADM();
        }

        public int NumberOfUserWithoutADM()
        {
            return userRepos.NumberOfUserWithoutADM();
        }

        public async Task UpdateAsync(User entity)
        {
            if (!await CheckEntityAsync(entity))
                throw new NotFoundException("User não existe");
            await userRepos.UpdateAsync(entity);
        }

        public async Task UpdateProfileAsync(User entity)
        {
            if (!await CheckEntityAsync(entity))
                throw new NotFoundException("User não existe");
            await userRepos.UpdateProfileAsync(entity);
        }
    }
}