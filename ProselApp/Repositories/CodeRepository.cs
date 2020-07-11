using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProselApp.Data;
using ProselApp.Libraries.Security;
using ProselApp.Models.AcessCode;
using ProselApp.Repositories.Interfaces;

namespace ProselApp.Repositories
{
    public class CodeRepository : ICodeRepository
    {
        private readonly ProselAppContext context;

        public CodeRepository(ProselAppContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(AccessCode accessCode)
        {
            context.AccessCode.Update(accessCode);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AccessCode accessCode)
        {
            context.AccessCode.Remove(accessCode);
            await context.SaveChangesAsync();
        }
        public async Task<AccessCode> SearchAndValidateCodeAsync(AccessCode accessCode)
        {
            return await context.AccessCode.AsNoTracking().
                        Where(cod => cod.User.Email.ToLower() == accessCode.User.Email.ToLower()
                        && cod.Key == Base64.Decode(accessCode.Key)
                        && cod.CodeType == accessCode.CodeType).FirstOrDefaultAsync();
        }
        public async Task<AccessCode> SearchCodeAsync(AccessCode accessCode)
        {
            return await context.AccessCode.AsNoTracking().
                        Where(cod => cod.CodeType == accessCode.CodeType
                        && cod.User.Email.ToLower() == accessCode.User.Email.ToLower()).FirstOrDefaultAsync();
        }
    }
}