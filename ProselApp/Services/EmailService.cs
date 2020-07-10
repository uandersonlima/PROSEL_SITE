using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProselApp.Models;
using ProselApp.Services.Interfaces;

namespace ProselApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient smtp;
        private readonly IConfiguration conf;

        public EmailService(SmtpClient smtp, IConfiguration conf)
        {
            this.smtp = smtp;
            this.conf = conf;
        }
        public async Task SendEmailRecoveryAsync(User user, string code_encrypted)
        {
            string corpoMsg = string.Format("<h1>Prosel - Law&Order</h1>" +
                               "Seu código de Recuperação é:" + $"<h2>{code_encrypted}</h2>");

            MailMessage mensagem = new MailMessage
            {
                From = new MailAddress(conf.GetValue<string>("Email:Username")),
                Subject = "Prosel - Law&Order - Codigo de Recuperação - " + user.Name,
                Body = corpoMsg,
                IsBodyHtml = true
            };
            mensagem.To.Add(user.Email);
            await smtp.SendMailAsync(mensagem);
        }
        public async Task SendEmailVerificationAsync(User user, string code_encrypted)
        {
            string corpoMsg = string.Format("<h1>Prosel - Law&Order</h1>" +
                                            "Seu código de Ativação é: " + $"<h2>{code_encrypted}</h2>");

            MailMessage mensagem = new MailMessage
            {
                From = new MailAddress(conf.GetValue<string>("Email:Username")),
                Subject = "Prosel - Law&Order - Código de Ativação - " + user.Name,
                Body = corpoMsg,
                IsBodyHtml = true
            };
            mensagem.To.Add(user.Email);
            await smtp.SendMailAsync(mensagem);
        }
    }
}