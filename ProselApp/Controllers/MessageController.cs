using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProselApp.Libraries.Filter;

namespace ProselApp.Controllers
{
    public class MessageController:Controller
    {
        //[UserAuthorization]
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