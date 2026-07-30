using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.UserMessageDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers
{
    [Authorize]
    public class MessageController(UserManager<AppUser> _userManager,
                                    AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            ViewBag.fullName = $"{user.FirstName} {user.LastName}";

            var messages = await _context.UserMessages.Include(x=> x.Sender)
                .Where(x=> x.ReceiverId == user.Id)
                .ToListAsync();

            return View(messages); 
        }

        public IActionResult SendMAil( )
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMail(SendMailDto sendMailDto)
        {
            try
            { 
                if (!ModelState.IsValid)
                    return View(sendMailDto);

                // Giriş yapan kullanıcı
                var sender = await _userManager.FindByNameAsync(User.Identity!.Name!);

                // Alıcı
                var receiver = await _userManager.FindByEmailAsync(sendMailDto.ReceiverMail);

                if (receiver == null)
                {
                    ModelState.AddModelError(nameof(sendMailDto.ReceiverMail),
                        "Bu e-posta adresine sahip kullanıcı bulunamadı.");

                    return View(sendMailDto);
                }

                var message = new UserMessage
                {
                    Subject = sendMailDto.Subject,
                    Body = sendMailDto.Body,
                    SendDate = DateTime.Now,
                    SenderId = sender!.Id,
                    ReceiverId = receiver.Id,
                    IsRead = false,
                    IsImportant = false
                };

                _context.UserMessages.Add(message);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Mesaj başarıyla gönderildi.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Mesaj gönderilirken beklenmeyen bir hata oluştu.");

                return View(sendMailDto);
            }
        }

        public async Task<IActionResult> Detail(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var message = await _context.UserMessages
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync(x => x.Id == id && x.ReceiverId == user!.Id);

            if (message == null)
                return NotFound();

            if (!message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(message);
        }
    }
}
