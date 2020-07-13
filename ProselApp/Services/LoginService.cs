using ProselApp.Models;
using ProselApp.Services.Interfaces;
using Newtonsoft.Json;

namespace ProselApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly string Key = "cf6cf0fcbde280ad01a1df9042ae0b0b";
        private readonly ISessionService session;

        public LoginService(ISessionService session)
        {
            this.session = session;
        }

        public User GetUser()
        {
            if (session.Check(Key))
            {
                string usuarioJSONString = session.Get(Key);
                var x = JsonConvert.DeserializeObject<User>(usuarioJSONString);
                return x;
            }
            else
            {
                return null;
            }
        }
        public void Login(User user)
        {
            string userJSON = JsonConvert.SerializeObject(user);
            session.Register(Key, userJSON);
        }
        public void Logout()
        {
            session.DeleteAll();
        }
    }
}