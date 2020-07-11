using System;
using System.Threading.Tasks;
using ProselApp.Libraries.Security;
using ProselApp.Libraries.Text;
using ProselApp.Models;
using ProselApp.Models.AcessCode;
using ProselApp.Models.Const;
using ProselApp.Repositories.Interfaces;
using ProselApp.Services.Interfaces;

namespace ProselApp.Services
{
    public class CodeService : ICodeService
    {
        private readonly ICodeRepository codeRepos;
        private readonly IEmailService emailSvc;

        public CodeService(ICodeRepository codeRepos, IEmailService emailSvc)
        {
            this.codeRepos = codeRepos;
            this.emailSvc = emailSvc;
        }

        public async Task AddAsync(AccessCode acessCode)
        {
            var previousCode = await SearchCodeAsync(acessCode);
            if (!(previousCode is null))
            {
                await DeleteAsync(previousCode);
            }
            await codeRepos.AddAsync(acessCode);
        }
        public bool CodeIsValid(string KeyCrip) => Base64.IsBase64(KeyCrip);
        public async Task CreateNewKeyAsync(User entity, string codeType)
        {
            var Key = GenKey.GetUniqueKey(8);
            var keyCrip = Base64.Encode(Key);
            AccessCode newCode = new AccessCode
            {
                Key = Key,
                UserCpf = entity.Cpf,
                User = entity,
                CodeType = codeType,
                GenDate = DateTime.Now
            };
            if (codeType == CodeType.Verification)
            {
                await emailSvc.SendEmailVerificationAsync(entity, keyCrip);
            }
            else if (codeType == CodeType.Recovery)
            {
                await emailSvc.SendEmailRecoveryAsync(entity, keyCrip);
            }
            await AddAsync(newCode);
        }

        public async Task DeleteAsync(AccessCode acessCode) => await codeRepos.DeleteAsync(acessCode);

        public async Task<TimeSpan> ElapsedTimeAsync(AccessCode acessCode)
        {
            var previousCodigo = await SearchCodeAsync(acessCode);
            if (!(previousCodigo is null)) 
            {               
                return DateTime.Now.Subtract(previousCodigo.GenDate);
            }
            return new TimeSpan(0, 15, 0);
        }

        public async Task<AccessCode> SearchAndValidateCodeAsync(AccessCode entity) => await codeRepos.SearchAndValidateCodeAsync(entity);

        public async Task<AccessCode> SearchCodeAsync(AccessCode entity) => await codeRepos.SearchCodeAsync(entity);
    }
}