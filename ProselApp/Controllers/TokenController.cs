using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProselApp.Libraries.Filter;
using ProselApp.Libraries.Lang;
using ProselApp.Models.Const;
using ProselApp.Services.Interfaces;

namespace ProselApp.Controllers
{
    public class TokenController:Controller
    {
        private readonly ITokenService tokenSvc;

        public TokenController(ITokenService tokenSvc)
        {
            this.tokenSvc = tokenSvc;
        }

        //[UserAuthorization(AccessType.Administrator)]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GerarNovoToken()
        {
            
            var elapsedTime = await tokenSvc.ElapsedTimeLastTokenAsync();
            var _1day = new TimeSpan(23, 59, 59);
            TempData["MSG_A"] = string.Format(MSG.MSG_E016, _1day.Subtract(elapsedTime));

            if (elapsedTime >= _1day)
            {
                TempData["MSG_A"] = null;
                await tokenSvc.CreateNewKeyAsync();
                TempData["MSG_S"] = MSG.MSG_S009;
            }
            return RedirectToAction("novousuario","user");
        }
    }
}