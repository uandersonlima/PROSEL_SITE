using System;
using System.Threading.Tasks;
using ProselApp.Models;
using ProselApp.Models.AcessCode;

namespace ProselApp.Services.Interfaces
{
    public interface ICodeService
    {
        Task AddAsync(AccessCode acessCode);
        bool CodeIsValid(string KeyCrip);
        Task CreateNewKeyAsync(User entity, string codeType);
        Task DeleteAsync(AccessCode acessCode);
        Task<TimeSpan> ElapsedTimeAsync(AccessCode acessCode);
        Task<AccessCode> SearchCodeAsync(AccessCode entity);
        Task<AccessCode> SearchAndValidateCodeAsync(AccessCode entity);
    }
}