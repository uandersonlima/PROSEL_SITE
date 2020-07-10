using Microsoft.AspNetCore.Http;
using ProselApp.Services.Interfaces;

namespace ProselApp.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor context;

        public SessionService(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public bool Check(string key)
        {
            if (context.HttpContext.Session.GetString(key) == null)
            {
                return false;
            }
            return true;
        }
        public void Delete(string key)
        {
            context.HttpContext.Session.Remove(key);
        }

        public void DeleteAll()
        {
            context.HttpContext.Session.Clear();
        }
        public string Get(string key)
        {
            return context.HttpContext.Session.GetString(key);
        }
        public void Update(string key, string value)
        {
            if (Check(key))
            {
                context.HttpContext.Session.Remove(key);
            }
            context.HttpContext.Session.SetString(key, value);
        }
        public void Register(string key, string value)
        {
            context.HttpContext.Session.SetString(key, value);
        }
    }
}