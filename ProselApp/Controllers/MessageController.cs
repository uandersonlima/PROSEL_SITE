using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProselApp.Controllers
{
    public class MessageController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Sender()
        {
            var status = false;


            return Json(status);
        }
    }
}