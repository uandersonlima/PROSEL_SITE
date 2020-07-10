using System.Threading.Tasks;
using ProselApp.Models;

namespace ProselApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(User user, string code_encrypted);
        Task SendEmailRecoveryAsync(User user, string code_encrypted);
    }
}