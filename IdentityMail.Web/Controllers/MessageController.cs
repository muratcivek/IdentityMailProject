using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.UserMessageDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class MessageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public MessageController(
            UserManager<AppUser> userManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        // =========================================================
        // ORTAK METOTLAR
        // =========================================================

        private async Task<AppUser?> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }


        private async Task LoadSidebarAsync(AppUser user)
        {
            ViewBag.fullName =
                $"{user.FirstName} {user.LastName}";

            ViewBag.profileImage =
                user.ProfileImageUrl;

            // Çöp kutusundaki mesajları okunmamış sayısına dahil etmiyoruz.
            ViewBag.unreadCount =
                await _context.UserMessages
                    .CountAsync(x =>
                        x.ReceiverId == user.Id &&
                        !x.IsRead &&
                        !x.IsDeletedByReceiver);
        }


        // =========================================================
        // GELEN KUTUSU
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Index(
      string? search,
      DateTime? startDate,
      DateTime? endDate,
      MessageCategory? category,
      string? readStatus,
      bool importantOnly = false,
      string sort = "newest",
      int page = 1)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            if (page < 1)
                page = 1;

            const int pageSize = 10;

            var query = _context.UserMessages
                .Include(x => x.Sender)
                .Where(x =>
                    x.ReceiverId == user.Id &&
                    !x.IsDeletedByReceiver &&
                    !x.IsDraft)
                .AsQueryable();


            // GÖNDEREN ADI / KONU ARAMA
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Subject.Contains(search) ||
                    x.Sender.FirstName.Contains(search) ||
                    x.Sender.LastName.Contains(search) ||
                    (x.Sender.FirstName + " " + x.Sender.LastName)
                        .Contains(search));
            }


            // BAŞLANGIÇ TARİHİ
            if (startDate.HasValue)
            {
                query = query.Where(x =>
                    x.SendDate >= startDate.Value.Date);
            }


            // BİTİŞ TARİHİ
            if (endDate.HasValue)
            {
                var nextDay =
                    endDate.Value.Date.AddDays(1);

                query = query.Where(x =>
                    x.SendDate < nextDay);
            }


            // KATEGORİ
            if (category.HasValue)
            {
                query = query.Where(x =>
                    x.Category == category.Value);
            }


            // OKUNDU / OKUNMADI
            if (readStatus == "read")
            {
                query = query.Where(x =>
                    x.IsRead);
            }
            else if (readStatus == "unread")
            {
                query = query.Where(x =>
                    !x.IsRead);
            }


            // SADECE ÖNEMLİ
            if (importantOnly)
            {
                query = query.Where(x =>
                    x.IsImportant);
            }


            // TOPLAM SONUÇ
            var totalCount =
                await query.CountAsync();


            // SIRALAMA
            query = sort == "oldest"
                ? query.OrderBy(x => x.SendDate)
                : query.OrderByDescending(x => x.SendDate);


            // SAYFALAMA
            var messages = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var model = new InboxViewModel
            {
                Messages = messages,

                Search = search,

                StartDate = startDate,

                EndDate = endDate,

                Category = category,

                ReadStatus = readStatus,

                ImportantOnly = importantOnly,

                Sort = sort,

                Page = page,

                PageSize = pageSize,

                TotalCount = totalCount
            };

            return View(model);
        }


        // =========================================================
        // GÖNDERİLENLER
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Sent()
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            var messages =
                await _context.UserMessages
                    .Include(x => x.Receiver)
                    .Where(x =>
    x.SenderId == user.Id &&
    !x.IsDeletedBySender &&
    !x.IsDraft)
                    .OrderByDescending(x => x.SendDate)
                    .ToListAsync();

            return View(messages);
        }


        // =========================================================
        // YENİ MESAJ
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> SendMail()
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            return View(new SendMailDto());
        }


        // =========================================================
        // MESAJ GÖNDER
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMail(SendMailDto model)
        {
            var sender = await GetCurrentUserAsync();

            if (sender == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(sender);

            if (string.IsNullOrWhiteSpace(model.ReceiverMail))
            {
                ModelState.AddModelError(
                    nameof(model.ReceiverMail),
                    "Alıcı e-posta adresi zorunludur.");
            }

            if (string.IsNullOrWhiteSpace(model.Subject))
            {
                ModelState.AddModelError(
                    nameof(model.Subject),
                    "Konu zorunludur.");
            }

            if (string.IsNullOrWhiteSpace(model.Body))
            {
                ModelState.AddModelError(
                    nameof(model.Body),
                    "Mesaj içeriği zorunludur.");
            }

            if (!ModelState.IsValid)
                return View(model);

            var receiver =
                await _userManager.FindByEmailAsync(model.ReceiverMail!);

            if (receiver == null)
            {
                ModelState.AddModelError(
                    nameof(model.ReceiverMail),
                    "Bu e-posta adresine sahip kayıtlı kullanıcı bulunamadı.");

                return View(model);
            }

            if (receiver.Id == sender.Id)
            {
                ModelState.AddModelError(
                    nameof(model.ReceiverMail),
                    "Kendinize mesaj gönderemezsiniz.");

                return View(model);
            }

            UserMessage message;

            if (model.DraftId.HasValue)
            {
                message = await _context.UserMessages
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.DraftId.Value &&
                        x.SenderId == sender.Id &&
                        x.IsDraft);

                if (message == null)
                    return NotFound();

                message.IsDraft = false;
            }
            else
            {
                message = new UserMessage
                {
                    SenderId = sender.Id
                };

                _context.UserMessages.Add(message);
            }

            message.ReceiverId = receiver.Id;
            message.Subject = model.Subject!;
            message.Body = model.Body!;
            message.Category = model.Category;

            message.SendDate = DateTime.Now;
            message.DraftUpdatedDate = null;

            message.IsRead = false;
            message.IsImportant = false;

            message.IsDeletedBySender = false;
            message.IsDeletedByReceiver = false;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Mesaj başarıyla gönderildi.";

            return RedirectToAction(nameof(Sent));
        }


        // =========================================================
        // MESAJ DETAY
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            var message =
                await _context.UserMessages
                    .Include(x => x.Sender)
                    .Include(x => x.Receiver)
                    .FirstOrDefaultAsync(x =>
                        x.Id == id &&
                        (
                            x.ReceiverId == user.Id ||
                            x.SenderId == user.Id
                        ));

            if (message == null)
                return NotFound();

            var isSender =
                message.SenderId == user.Id;

            // Kullanıcı kendi tarafında bu mesajı çöpe attıysa
            // normal Detail ekranından açılmasını engelliyoruz.
            if (isSender && message.IsDeletedBySender)
                return RedirectToAction(nameof(Trash));

            if (!isSender && message.IsDeletedByReceiver)
                return RedirectToAction(nameof(Trash));

            ViewBag.IsSender = isSender;

            // Mesajı sadece alıcı açtığında okundu yap.
            if (!isSender && !message.IsRead)
            {
                message.IsRead = true;

                await _context.SaveChangesAsync();

                // Sidebar okunmamış sayısını güncelle.
                await LoadSidebarAsync(user);
            }

            return View(message);
        }


        // =========================================================
        // YANITLA
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Reply(int id)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            var originalMessage =
                await _context.UserMessages
                    .Include(x => x.Sender)
                    .FirstOrDefaultAsync(x =>
                        x.Id == id &&
                        x.ReceiverId == user.Id &&
                        !x.IsDeletedByReceiver);

            if (originalMessage == null)
                return NotFound();

            var subject =
                originalMessage.Subject;

            if (!subject.StartsWith(
                    "RE:",
                    StringComparison.OrdinalIgnoreCase))
            {
                subject =
                    "RE: " + subject;
            }

            var model =
                new SendMailDto
                {
                    ReceiverMail =
                        originalMessage.Sender.Email!,

                    Subject =
                        subject,

                    Category =
                        originalMessage.Category
                };

            return View("SendMail", model);
        }


        // =========================================================
        // ÖNEMLİ / ÖNEMSİZ
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleImportant(
      int id,
      string? returnUrl = null)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var message =
                await _context.UserMessages
                    .FirstOrDefaultAsync(x =>
                        x.Id == id &&
                        x.ReceiverId == user.Id &&
                        !x.IsDeletedByReceiver &&
                        !x.IsDraft);

            if (message == null)
                return NotFound();

            message.IsImportant =
                !message.IsImportant;

            await _context.SaveChangesAsync();


            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }


            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // ÖNEMLİ MESAJLAR
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Important()
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            var messages =
                await _context.UserMessages
                    .Include(x => x.Sender)
                    .Include(x => x.Receiver)
                  .Where(x =>
    x.ReceiverId == user.Id &&
    x.IsImportant &&
    !x.IsDeletedByReceiver &&
    !x.IsDraft)
                    .OrderByDescending(x => x.SendDate)
                    .ToListAsync();

            return View(messages);
        }


        // =========================================================
        // ÇÖP KUTUSUNA TAŞI
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToTrash(int id)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    (x.SenderId == user.Id ||
                     x.ReceiverId == user.Id));

            if (message == null)
                return NotFound();

            var isSender = message.SenderId == user.Id;

            if (isSender)
            {
                message.IsDeletedBySender = true;
            }
            else
            {
                message.IsDeletedByReceiver = true;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Mesaj çöp kutusuna taşındı.";

            return RedirectToAction(
                isSender ? nameof(Sent) : nameof(Index));
        }


        // =========================================================
        // ÇÖP KUTUSU
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            var messages =
                await _context.UserMessages
                    .Include(x => x.Sender)
                    .Include(x => x.Receiver)
                    .Where(x =>
                        (
                            x.ReceiverId == user.Id &&
                            x.IsDeletedByReceiver
                        )
                        ||
                        (
                            x.SenderId == user.Id &&
                            x.IsDeletedBySender
                        ))
                    .OrderByDescending(x => x.SendDate)
                    .ToListAsync();

            return View(messages);
        }


        // =========================================================
        // ÇÖP KUTUSUNDAN GERİ YÜKLE
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(
            int id)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var message =
                await _context.UserMessages
                    .FirstOrDefaultAsync(x =>
                        x.Id == id &&
                        (
                            (
                                x.ReceiverId == user.Id &&
                                x.IsDeletedByReceiver
                            )
                            ||
                            (
                                x.SenderId == user.Id &&
                                x.IsDeletedBySender
                            )
                        ));

            if (message == null)
                return NotFound();

            if (message.ReceiverId == user.Id)
            {
                message.IsDeletedByReceiver = false;
            }

            if (message.SenderId == user.Id)
            {
                message.IsDeletedBySender = false;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Mesaj başarıyla geri yüklendi.";

            return RedirectToAction(nameof(Trash));
        }


        // =========================================================
        // KALICI SİL
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermanentDelete(
            int id)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var message =
                await _context.UserMessages
                    .FirstOrDefaultAsync(x =>
                        x.Id == id &&
                        (
                            (
                                x.ReceiverId == user.Id &&
                                x.IsDeletedByReceiver
                            )
                            ||
                            (
                                x.SenderId == user.Id &&
                                x.IsDeletedBySender
                            )
                        ));

            if (message == null)
                return NotFound();

            /*
             * Kullanıcının kendi kopyasını kalıcı olarak silmiş
             * kabul ediyoruz.
             *
             * Diğer taraf hâlâ mesajı kullanıyorsa kayıt DB'de
             * kalmaya devam eder.
             */

            if (message.ReceiverId == user.Id)
            {
                message.IsDeletedByReceiver = true;
            }

            if (message.SenderId == user.Id)
            {
                message.IsDeletedBySender = true;
            }

            /*
             * İki kullanıcı da kendi tarafında mesajı sildiyse
             * artık gerçek DB kaydını kaldırabiliriz.
             */
            if (message.IsDeletedBySender &&
                message.IsDeletedByReceiver)
            {
                _context.UserMessages.Remove(message);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Mesaj kalıcı olarak silindi.";

            return RedirectToAction(nameof(Trash));
        }

        // TASLAKLAR
        [HttpGet]
        public async Task<IActionResult> Drafts()
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            var drafts = await _context.UserMessages
                .Include(x => x.Receiver)
                .Where(x =>
                    x.SenderId == user.Id &&
                    x.IsDraft &&
                    !x.IsDeletedBySender)
                .OrderByDescending(x => x.DraftUpdatedDate)
                .ToListAsync();

            return View(drafts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(SendMailDto model)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            AppUser? receiver = null;

            if (!string.IsNullOrWhiteSpace(model.ReceiverMail))
            {
                receiver = await _userManager.FindByEmailAsync(model.ReceiverMail);
            }

            UserMessage draft;

            if (model.DraftId.HasValue)
            {
                draft = await _context.UserMessages
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.DraftId.Value &&
                        x.SenderId == user.Id &&
                        x.IsDraft);

                if (draft == null)
                    return NotFound();
            }
            else
            {
                draft = new UserMessage
                {
                    SenderId = user.Id,
                    IsDraft = true,
                    IsRead = false,
                    IsImportant = false,
                    IsDeletedBySender = false,
                    IsDeletedByReceiver = false,
                    SendDate = DateTime.Now
                };

                _context.UserMessages.Add(draft);
            }

            draft.Subject = model.Subject ?? string.Empty;
            draft.Body = model.Body ?? string.Empty;
            draft.Category = model.Category;
            draft.ReceiverId = receiver?.Id;
            draft.DraftUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Taslak kaydedildi.";

            return RedirectToAction(nameof(Drafts));
        }

        [HttpGet]
        public async Task<IActionResult> EditDraft(int id)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            var draft = await _context.UserMessages
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.SenderId == user.Id &&
                    x.IsDraft);

            if (draft == null)
                return NotFound();

            var model = new SendMailDto
            {
                DraftId = draft.Id,
                ReceiverMail = draft.Receiver?.Email,
                Subject = draft.Subject,
                Body = draft.Body,
                Category = draft.Category
            };

            return View("SendMail", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDraft(int id)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var draft = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.SenderId == user.Id &&
                    x.IsDraft);

            if (draft == null)
                return NotFound();

            _context.UserMessages.Remove(draft);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Taslak silindi.";

            return RedirectToAction(nameof(Drafts));
        }

        [HttpGet]
        public async Task<IActionResult> Report(int id)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.ReceiverId == user.Id);

            if (message == null)
                return NotFound();

            var alreadyReported =
                await _context.MessageReports
                    .AnyAsync(x =>
                        x.MessageId == id &&
                        x.ReporterId == user.Id);

            if (alreadyReported)
            {
                TempData["Error"] =
                    "Bu mesajı daha önce şikayet ettiniz.";

                return RedirectToAction(
                    nameof(Detail),
                    new { id });
            }

            return View(
                new ReportMessageDto
                {
                    MessageId = id
                });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(
            ReportMessageDto dto)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return Unauthorized();

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.MessageId &&
                    x.ReceiverId == user.Id);

            if (message == null)
                return NotFound();

            var alreadyReported =
                await _context.MessageReports
                    .AnyAsync(x =>
                        x.MessageId == dto.MessageId &&
                        x.ReporterId == user.Id);

            if (alreadyReported)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Bu mesajı daha önce şikayet ettiniz.");
            }

            if (!ModelState.IsValid)
                return View(dto);

            var report = new MessageReport
            {
                MessageId = dto.MessageId,
                ReporterId = user.Id,
                Reason = dto.Reason,
                Description = dto.Description,
                Status = ReportStatus.Pending,
                CreatedDate = DateTime.Now
            };

            _context.MessageReports.Add(report);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Şikayetiniz yönetici incelemesine gönderildi.";

            return RedirectToAction(
                nameof(Detail),
                new { id = dto.MessageId });
        }

        // BENİM ŞİKAYETLERİM
        [HttpGet]
        public async Task<IActionResult> MyReports()
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
                return RedirectToAction("Login", "Auth");

            await LoadSidebarAsync(user);

            var reports = await _context.MessageReports
                .Include(x => x.Message)
                    .ThenInclude(x => x.Sender)
                .Include(x => x.Message)
                    .ThenInclude(x => x.Receiver)
                .Where(x => x.ReporterId == user.Id)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(reports);
        }
    }
}