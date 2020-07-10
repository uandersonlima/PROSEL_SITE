using ProselApp.Models;

namespace ProselApp.Services.Interfaces
{
    public interface ILoginService
    {
        void Login(User user);
        User GetUser();
        void Logout();
    }
}