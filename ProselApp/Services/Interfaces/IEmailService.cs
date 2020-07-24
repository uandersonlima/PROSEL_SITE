using System.Collections.Generic;
using System.Threading.Tasks;
using ProselApp.Models;

namespace ProselApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendTokenToOwnerAsync(Token token);
        Task NotifyAllToEmailAsync(Message msg, List<string> usersEmails);
        Task SendEmailVerificationAsync(User user, string code_encrypted);
        Task SendEmailRecoveryAsync(User user, string code_encrypted);
    }
}