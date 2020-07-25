using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProselApp.Libraries.Filter;
using ProselApp.Libraries.Lang;
using ProselApp.Models;
using ProselApp.Services;
using ProselApp.Services.Interfaces;

namespace ProselApp.Controllers
{

    public class MessageController : Controller
    {
        private readonly IMessageService msgSvc;
        private readonly IEmailService emailSvc;
        private readonly IHubContext<NotificationService> hubcontext;
        private readonly IUserService userSvc;

        public MessageController(IMessageService msgSvc, IEmailService emailSvc, IHubContext<NotificationService> hubcontext, IUserService userSvc)
        {
            this.msgSvc = msgSvc;
            this.emailSvc = emailSvc;
            this.hubcontext = hubcontext;
            this.userSvc = userSvc;
        }

        [HttpGet, UserAuthorization]
        public async Task<IActionResult> Index(string pesquisa)
        {
            var msgs = await msgSvc.GetAllAsync(pesquisa);
            return View(msgs.Where(msg => !msg.isDeleted).ToList());
        }

        [HttpGet, UserAuthorization]
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
        [HttpGet, UserAuthorization]
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
        [HttpPost, UserAuthorization]
        public async Task<IActionResult> Delete(int messagecode)
        {
            var msg = await msgSvc.GetByCodeAsync(messagecode);
            await msgSvc.LogicallyDeleteAsync(msg);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet, UserAuthorization]
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
        [HttpPost, UserAuthorization]
        public async Task<IActionResult> MarkAsRead(int messagecode)
        {
            var msg = await msgSvc.GetByCodeAsync(messagecode);
            msg.ViewedTime = DateTime.Now;
            await msgSvc.UpdateMsgAsync(msg);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost, UserAuthorization]
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
        [HttpPost, UserAuthorization]
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
            if (ModelState.IsValid)
            {
                TempData["MSG_S"] = MSG.MSG_S010;
                await msgSvc.AddAsync(message);
                var userEmails = userSvc.GetAllUserAsync().Result.Where(user => user.Receive_emails).Select(user => user.Email).ToList();
                if (userEmails.Count > 0)
                {
                    await emailSvc.NotifyAllToEmailAsync(message, userEmails);
                }
                await hubcontext.Clients.All.SendAsync("novamsg");
                return LocalRedirect("/#contact");
            }
            return LocalRedirect("/#contact");
        }

        [HttpGet, Route("Trash"), UserAuthorization]
        public async Task<IActionResult> Trash(string pesquisa)
        {
            var msgs = await msgSvc.GetAllAsync(pesquisa);
            return View(msgs.Where(msg => msg.isDeleted).ToList());
        }
        [HttpGet, UserAuthorization]
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
        [HttpGet, UserAuthorization]
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

        [HttpPost, UserAuthorization]
        public async Task<IActionResult> DeleteTrash(int messagecode)
        {
            var msg = await msgSvc.GetByCodeAsync(messagecode);
            await msgSvc.DeleteAsync(msg);
            return RedirectToAction(nameof(Trash));
        }
        [HttpGet, UserAuthorization]
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
        [HttpPost, UserAuthorization]
        public async Task<IActionResult> MarkAsReadTrash(int messagecode)
        {
            var msg = await msgSvc.GetByCodeAsync(messagecode);
            msg.ViewedTime = DateTime.Now;
            await msgSvc.UpdateMsgAsync(msg);
            return RedirectToAction(nameof(Trash));
        }
        [HttpPost, UserAuthorization]
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
        [HttpPost, UserAuthorization]
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

        [HttpGet, Route("Trash/Message"), UserAuthorization]
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
        [HttpGet, UserAuthorization]
        public async Task<IActionResult> MarkAll()
        {
            var msgs = await msgSvc.GetAllAsync(null);
            msgs.ForEach(msg => msg.ViewedTime = DateTime.Now);
            await msgSvc.UpdateMultipleMsgsAsync(msgs);
            return RedirectToAction(nameof(Index));
        }

    }
}