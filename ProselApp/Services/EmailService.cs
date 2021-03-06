using System.Collections.Generic;
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

        public async Task NotifyAllToEmailAsync(Message msg, List<string> usersEmails)
        {
            string corpoMsg = string.Format("<h1>Prosel - Law&Order</h1>" +
                               "Nova mensagem de:" + $"<h2>{msg.Sender} - {msg.Email}</h2>");

            MailMessage mensagem = new MailMessage
            {
                From = new MailAddress(conf.GetValue<string>("Email:Username")),
                Subject = "Prosel - Law&Order - Nova mensagem recebida - " + msg.Sender,
                Body = corpoMsg,
                IsBodyHtml = true
            };
            usersEmails.ForEach(email => mensagem.To.Add(email));
            await smtp.SendMailAsync(mensagem);
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

        public async Task SendTokenToOwnerAsync(Token token)
        {
            string corpoMsg = string.Format("<h1>Prosel - Law&Order</h1>" +
                                "Novo token solicitado externamente: " + $"<h2>{token.SecurityToken}</h2>");
            MailMessage mensagem = new MailMessage
            {
                From = new MailAddress(conf.GetValue<string>("Email:Username")),
                Subject = "Nova solicitação de token recebida",
                Body = corpoMsg,
                IsBodyHtml = true
            };
            mensagem.To.Add(conf.GetValue<string>("Email:Username"));
            await smtp.SendMailAsync(mensagem);
        }
    }
}