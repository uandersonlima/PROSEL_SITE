using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProselApp.Libraries.Lang;
using ProselApp.Models;
using ProselApp.Services;
using ProselApp.Services.Interfaces;

namespace ProselApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService msgSvc;
        private readonly IHubContext<MsgHub> msgHubContext;
        public MessageController(IMessageService msgSvc, IHubContext<MsgHub> msgHubContext)
        {
            this.msgSvc = msgSvc;
            this.msgHubContext = msgHubContext;
        }

        //[UserAuthorization]
        public async Task<IActionResult> Index(string pesquisa)
        {
            var msgs = await msgSvc.GetAllAsync(pesquisa);
            return View(msgs.Where(msg => !msg.isDeleted).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> MessagePage(int? messagecode)
        {
            if (!messagecode.HasValue)
            {
                return NotFound();
            }
            var msg = await msgSvc.GetByCodeAsync(messagecode.Value);
            if (msg.ViewedTime == null)
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
            await msgSvc.LogicallyDeleteAsync(msg);
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
        public async Task<JsonResult> DeleteMultiple(List<int> messagecodes)
        {
            var msgs = new List<Message>();
            if (messagecodes.Count != 0)
            {
                msgs = await msgSvc.GetMultipleMsgsAsync(messagecodes);
                await msgSvc.LogicallyDeleteMultipleAsync(msgs);
            }
            return Json("/Message");
        }
        [HttpPost]
        public async Task<JsonResult> MarkMultiple(List<int> messagecodes)
        {
            var msgs = new List<Message>();
            if (messagecodes.Count != 0)
            {
                msgs = await msgSvc.GetMultipleMsgsAsync(messagecodes);
                msgs.ForEach(msg => { if (msg.ViewedTime == null) { msg.ViewedTime = DateTime.Now; } });
                await msgSvc.UpdateMultipleMsgsAsync(msgs);
            }
            return Json("/Message");
        }
        [HttpPost]
        public async Task<IActionResult> Sender(Message message)
        {
            var hub = new MsgHub();
            if (ModelState.IsValid)
            {
                TempData["MSG_S"] = MSG.MSG_S010;
                await msgSvc.AddAsync(message);
                hub.Notify();
                msgHubContext.Clients.All.SendAsync("newMsg");
                return LocalRedirect("/#contact");
            }
            return LocalRedirect("/#contact");
        }

        [HttpGet, Route("Trash")]
        public async Task<IActionResult> Trash(string pesquisa)
        {
            var msgs = await msgSvc.GetAllAsync(pesquisa);
            return View(msgs.Where(msg => msg.isDeleted).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> RestoreMessage(int? messagecode)
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
            msg.isDeleted = false;
            await msgSvc.UpdateMsgAsync(msg);

            return RedirectToAction("Trash", new { });
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTrash(int? messagecode)
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
        public async Task<IActionResult> DeleteTrash(int messagecode)
        {
            var msg = await msgSvc.GetByCodeAsync(messagecode);
            await msgSvc.DeleteAsync(msg);
            return RedirectToAction(nameof(Trash));
        }
        [HttpGet]
        public async Task<IActionResult> MarkAsReadTrash(int? messagecode)
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
        public async Task<IActionResult> MarkAsReadTrash(int messagecode)
        {
            var msg = await msgSvc.GetByCodeAsync(messagecode);
            msg.ViewedTime = DateTime.Now;
            await msgSvc.UpdateMsgAsync(msg);
            return RedirectToAction(nameof(Trash));
        }
        [HttpPost]
        public async Task<JsonResult> DeleteMultipleTrash(List<int> messagecodes)
        {
            var msgs = new List<Message>();
            if (messagecodes.Count != 0)
            {
                msgs = await msgSvc.GetMultipleMsgsAsync(messagecodes);
                await msgSvc.DeleteMultipleAsync(msgs);
            }
            return Json("/Trash");
        }
        [HttpPost]
        public async Task<JsonResult> MarkMultipleTrash(List<int> messagecodes)
        {
            var msgs = new List<Message>();
            if (messagecodes.Count != 0)
            {
                msgs = await msgSvc.GetMultipleMsgsAsync(messagecodes);
                msgs.ForEach(msg => { if (msg.ViewedTime == null) { msg.ViewedTime = DateTime.Now; } });
                await msgSvc.UpdateMultipleMsgsAsync(msgs);
            }
            return Json("/Trash");
        }

        [HttpGet, Route("Trash/Message")]
        public async Task<IActionResult> MessagePageTrash(int? messagecode)
        {
            if (!messagecode.HasValue)
            {
                return NotFound();
            }
            var msg = await msgSvc.GetByCodeAsync(messagecode.Value);
            if (msg.ViewedTime == null)
            {
                msg.ViewedTime = DateTime.Now;
                await msgSvc.UpdateMsgAsync(msg);
            }
            return View(msg);
        }

    }
}