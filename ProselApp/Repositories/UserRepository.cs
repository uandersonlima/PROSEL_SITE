using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProselApp.Data;
using ProselApp.Models;
using ProselApp.Models.Const;
using ProselApp.Repositories.Interfaces;
using ProselApp.Services.Exceptions;

namespace ProselApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProselAppContext context;

        public UserRepository(ProselAppContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(User entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(User entity)
        {
            try
            {
                context.Update(entity);
                context.Entry(entity).Property(a => a.Name).IsModified = false;
                context.Entry(entity).Property(a => a.AccountStatus).IsModified = false;
                context.Entry(entity).Property(a => a.Telephone).IsModified = false;
                context.Entry(entity).Property(a => a.Email).IsModified = false;
                context.Entry(entity).Property(a => a.AccessType).IsModified = false;

                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task<bool> CheckEntityAsync(User entity)
        {
            return await context.User.AnyAsync(user => user.Cpf == entity.Cpf);
        }

        public async Task DeleteAsync(User entity)
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
        public async Task<List<User>> GetByPermission(string perm)
        {
            return await context.User.Where(user => user.AccessType == perm).ToListAsync();;
        }
        public async Task<User> GetByCpfAsync(string cpf)
        {
            return await context.User.AsNoTracking()
                .Where(user => user.Cpf == cpf)
                .FirstOrDefaultAsync();
        }

        //não pode ser assíncrono por causa do filtro
        public List<User> GetUserByEmail(string email)
        {
            return context.User.AsNoTracking().Where(a => a.Email.ToLower() == email.ToLower()).ToList();
        }
        public async Task<User> GetUserByLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            var user = await context.User.Where(m => m.Email.ToLower() == email.ToLower() && m.Password == password).FirstOrDefaultAsync();
            return user;
        }

        public async Task<int> NumberOfUserWithADM()
        {
            return await context.User.CountAsync();
        }

        public int NumberOfUserWithoutADM()
        {
            return context.User.Count(user => user.AccessType != AccessType.Administrator);
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {
                context.Update(entity);
                context.Entry(entity).Property(a => a.Password).IsModified = false;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task UpdateProfileAsync(User entity)
        {
            try
            {
                context.Update(entity);
                context.Entry(entity).Property(a => a.Password).IsModified = false;
                context.Entry(entity).Property(a => a.Email).IsModified = false;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
        public async Task<List<User>> GetAllUserAsync()
        {
           return await context.User.ToListAsync();
        }
    }
}