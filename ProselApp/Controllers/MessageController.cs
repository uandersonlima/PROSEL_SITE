using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProselApp.Libraries.Lang;
using ProselApp.Models;
using ProselApp.Services.Interfaces;

namespace ProselApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService msgSvc;
        public MessageController(IMessageService msgSvc)
        {
            this.msgSvc = msgSvc;
        }

        //[UserAuthorization]
        public async Task<IActionResult> Index(string pesquisa)
        {
            return View(await msgSvc.GetAllAsync(pesquisa));
        }

        [HttpGet]
        public async Task<IActionResult> MessagePage(int? messagecode)
        {
            if(!messagecode.HasValue)
            {
                return NotFound();
            }
            var msg = await msgSvc.GetByCodeAsync(messagecode.Value);
            if(msg.ViewedTime == null)
            {
                msg.ViewedTime = DateTime.Now;
                await msgSvc.UpdateMsgAsync(msg);
            }
            return View(msg);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? messagecode)
        {
            if (!messagecode.HasValue)
            {
                return NotFound();
            }
            var msg = await msgSvc.GetByCodeAsync(messagecode.Value);
            if (msg == null)
            {
                return NotFound();
            }
            return PartialView(msg);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int messagecode)
        {
            var msg = await msgSvc.GetByCodeAsync(messagecode);
            await msgSvc.DeleteAsync(msg);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> MarkAsRead(int? messagecode)
        {
            if (!messagecode.HasValue)
            {
                return NotFound();
            }
            var msg = await msgSvc.GetByCodeAsync(messagecode.Value);
            if (msg == null)
            {
                return NotFound();
            }
            return PartialView(msg);
        }
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int messagecode)
        {
            var msg = await msgSvc.GetByCodeAsync(messagecode);
            msg.ViewedTime = DateTime.Now;
            await msgSvc.UpdateMsgAsync(msg);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMultiple(List<int> messagecodes)
        {
            var msgs = new List<Message>();
            if (messagecodes.Count != 0)
            {
                msgs = await msgSvc.GetMultipleMsgsAsync(messagecodes);
                await msgSvc.DeleteMultipleAsync(msgs);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> MarkMultiple(List<int> messagecodes)
        {
            var msgs = new List<Message>();
            if (messagecodes.Count != 0)
            {
                msgs = await msgSvc.GetMultipleMsgsAsync(messagecodes);
                msgs.ForEach(msg => {msg.ViewedTime = DateTime.Now;});
                await msgSvc.UpdateMultipleMsgsAsync(msgs);
            }
            return PartialView(msgs);
        }
        [HttpPost]
        public async Task<IActionResult> Sender(Message message)
        {
            if (ModelState.IsValid)
            {
                TempData["MSG_S"] = MSG.MSG_S010;
                await msgSvc.AddAsync(message);
                return LocalRedirect("/#contact");
            }
            return LocalRedirect("/#contact");
        }
    }
}