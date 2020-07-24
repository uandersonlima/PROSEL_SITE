using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProselApp.Libraries.Filter;
using ProselApp.Libraries.Lang;
using ProselApp.Models;
using ProselApp.Models.Const;
using ProselApp.Services.Interfaces;

namespace ProselApp.Controllers
{
    public class TokenController : Controller
    {
        private readonly ITokenService tokenSvc;

        public TokenController(ITokenService tokenSvc)
        {
            this.tokenSvc = tokenSvc;
        }

        //[UserAuthorization(AccessType.Administrator)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await tokenSvc.GetAllTokens());
        }
        [HttpGet]
        public JsonResult NewToken()
        {
            var token = tokenSvc.NewToken();
            return Json(token);
        }
        [HttpGet]
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
            return RedirectToAction("novousuario", "user");
        }
        [HttpGet]
        public IActionResult AdicionarToken()
        {
            var token = tokenSvc.NewToken();
            return PartialView(token);
        }
        [HttpPost]
        public async Task<IActionResult> AdicionarToken(Token token)
        {
            token.CreationDate = DateTime.Now;
            await tokenSvc.AddAsync(token);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> EditToken(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            var token = await tokenSvc.GetByIdAsync(id.Value);
            if (token == null)
            {
                return NotFound();
            }
            return PartialView(token);
        }
        [HttpPost]
        public async Task<IActionResult> EditToken(int id, Token token)
        {
            await tokenSvc.UpdateAsync(token);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DeleteToken(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            var token = await tokenSvc.GetByIdAsync(id.Value);
            if (token == null)
            {
                return NotFound();
            }
            return PartialView(token);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteToken(int id)
        {
            await tokenSvc.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}