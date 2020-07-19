using System;
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProselApp.Data;
using ProselApp.Repositories;
using ProselApp.Repositories.Interfaces;
using ProselApp.Services.Interfaces;

namespace ProselApp.Services.IoC
{
    public class DependencyInjection
    {
        public static void Inject(IServiceCollection svc, IConfiguration conf)
        {
            svc.AddControllersWithViews().AddNewtonsoftJson(opt => { opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });
            svc.AddHttpContextAccessor();
            svc.AddSignalR();
            //Email - Gerenciamento
            svc.AddScoped<SmtpClient>(options =>
            {
                SmtpClient smtp = new SmtpClient()
                {
                    Host = conf.GetValue<string>("Email:ServerSMTP"),
                    Port = conf.GetValue<int>("Email:ServerPort"),
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(conf.GetValue<string>("Email:Username"), conf.GetValue<string>("Email:Password")),
                    EnableSsl = true
                };

                return smtp;
            });

            //Session - Configuration
            svc.AddMemoryCache(); //Guardar os dados na memória
            svc.AddSession(options =>
            {
                //// Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                //// Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            //Banco
            svc.AddDbContext<ProselAppContext>(x => x.UseSqlite(conf.GetConnectionString("DefaultConnection")));
            
            //Services
            svc.AddScoped<ICodeService, CodeService>();
            svc.AddScoped<IEmailService, EmailService>();
            svc.AddScoped<ILoginService, LoginService>();
            svc.AddScoped<IMessageService, MessageService>();
            svc.AddScoped<ISessionService, SessionService>();
            svc.AddScoped<ITokenService, TokenService>();
            svc.AddScoped<IUserService, UserService>();

            //Repositories
            svc.AddScoped<ICodeRepository, CodeRepository>();
            svc.AddScoped<IMessageRepository, MessageRepository>();
            svc.AddScoped<ITokenRepository, TokenRepository>();
            svc.AddScoped<IUserRepository, UserRepository>();

        }
    }
}