using Microsoft.AspNetCore.Mvc;

namespace ProselApp.Controllers
{
    public class MessageController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}