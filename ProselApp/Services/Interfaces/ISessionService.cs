namespace ProselApp.Services.Interfaces
{
    public interface ISessionService
    {
        bool Check(string key);
        void Delete(string key);
        void DeleteAll();
        string Get(string key);
        void Register(string key, string value);
        void Update(string key, string value);
    }
}