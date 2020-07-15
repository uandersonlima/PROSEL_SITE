using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProselApp.Libraries.Filter;
using ProselApp.Libraries.Lang;
using ProselApp.Models;
using ProselApp.Models.Const;
using ProselApp.Services.Interfaces;

namespace ProselApp.Controllers
{
    public class MessageController:Controller
    {
        private readonly IMessageService msgSvc;

        public MessageController(IMessageService msgSvc)
        {
            this.msgSvc = msgSvc;
        }

        [UserAuthorization]
        public IActionResult Index()
        {
            return View(); 
        }

        public IActionResult MessagePage()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Sender(Message message)
        {
            if(ModelState.IsValid)
            {
                TempData["MSG_S"] = MSG.MSG_S010;
                await msgSvc.AddAsync(message);       
                return LocalRedirect("/#contact");
            }           
            return LocalRedirect("/#contact");
        }
    }
}