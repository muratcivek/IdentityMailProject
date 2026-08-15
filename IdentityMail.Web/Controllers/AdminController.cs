using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminController(
            AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        // =====================================================
        // ADMIN DASHBOARD
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            ViewBag.fullName =
                $"{currentUser.FirstName} {currentUser.LastName}";

            ViewBag.profileImage =
                currentUser.ProfileImageUrl;


            // =================================================
            // KULLANICILAR
            // =================================================

            var totalUsers =
                await _userManager.Users
                    .CountAsync();

            var activeUsers =
                await _userManager.Users
                    .CountAsync(x => x.IsActive);


            // =================================================
            // TOPLAM MESAJ
            // =================================================

            // Taslakları gerçek gönderilmiş mesaj olarak saymıyoruz.
            var totalMessages =
                await _context.UserMessages
                    .CountAsync(x => !x.IsDraft);


            // =================================================
            // BUGÜN GÖNDERİLEN MESAJ
            // =================================================

            var today =
                DateTime.Today;

            var tomorrow =
                today.AddDays(1);

            var todayMessages =
                await _context.UserMessages
                    .CountAsync(x =>
                        !x.IsDraft &&
                        x.SendDate >= today &&
                        x.SendDate < tomorrow);


            // =================================================
            // OKUNMAMIŞ MESAJ
            // =================================================

            var unreadMessages =
                await _context.UserMessages
                    .CountAsync(x =>
                        !x.IsDraft &&
                        !x.IsRead &&
                        !x.IsDeletedByReceiver);


            // =================================================
            // ÇÖP KUTUSU
            // =================================================

            var trashMessages =
                await _context.UserMessages
                    .CountAsync(x =>
                        !x.IsDraft &&
                        (
                            x.IsDeletedBySender ||
                            x.IsDeletedByReceiver
                        ));


            // =================================================
            // EN FAZLA MESAJ GÖNDERENLER
            // =================================================

            var topSenders =
                await _context.UserMessages

                    .Where(x =>
                        !x.IsDraft)

                    .GroupBy(x => new
                    {
                        x.SenderId,

                        x.Sender.FirstName,

                        x.Sender.LastName,

                        x.Sender.Email
                    })

                    .Select(x =>
                        new TopSenderDto
                        {
                            FullName =
                                x.Key.FirstName + " " +
                                x.Key.LastName,

                            Email =
                                x.Key.Email ?? string.Empty,

                            MessageCount =
                                x.Count()
                        })

                    .OrderByDescending(x =>
                        x.MessageCount)

                    .Take(5)

                    .ToListAsync();


            // =================================================
            // KATEGORİ İSTATİSTİKLERİ
            // =================================================

            var categories =
                await _context.UserMessages

                    .Where(x =>
                        !x.IsDraft)

                    .GroupBy(x =>
                        x.Category)

                    .Select(x => new
                    {
                        Category = x.Key,

                        Count = x.Count()
                    })

                    .OrderByDescending(x =>
                        x.Count)

                    .ToListAsync();


            var categoryStatistics =
                categories
                    .Select(x =>
                        new CategoryStatisticDto
                        {
                            CategoryName =
                                x.Category.ToString(),

                            MessageCount =
                                x.Count,

                            Percentage =
                                totalMessages == 0
                                    ? 0
                                    : Math.Round(
                                        x.Count * 100.0 /
                                        totalMessages,
                                        1)
                        })
                    .ToList();


            // =================================================
            // DASHBOARD MODEL
            // =================================================

            var model =
                new AdminDashboardDto
                {
                    TotalUsers =
                        totalUsers,

                    ActiveUsers =
                        activeUsers,

                    TotalMessages =
                        totalMessages,

                    TodayMessages =
                        todayMessages,

                    UnreadMessages =
                        unreadMessages,

                    TrashMessages =
                        trashMessages,

                    TopSenders =
                        topSenders,

                    CategoryStatistics =
                        categoryStatistics
                };


            return View(model);
        }


        // =====================================================
        // KULLANICI YÖNETİMİ
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Users(string? search)
        {
            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            ViewBag.fullName =
                $"{currentUser.FirstName} {currentUser.LastName}";

            ViewBag.profileImage =
                currentUser.ProfileImageUrl;

            ViewBag.Search =
                search;


            var query =
                _userManager.Users
                    .AsQueryable();


            // ARAMA
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.FirstName.Contains(search) ||
                    x.LastName.Contains(search) ||
                    (x.FirstName + " " + x.LastName).Contains(search) ||
                    (x.Email != null && x.Email.Contains(search)) ||
                    (x.UserName != null && x.UserName.Contains(search)));
            }


            var users =
                await query
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToListAsync();


            var model =
                new List<AdminUserDto>();


            foreach (var user in users)
            {
                var roles =
                    await _userManager.GetRolesAsync(user);

                model.Add(
                    new AdminUserDto
                    {
                        Id =
                            user.Id,

                        FullName =
                            $"{user.FirstName} {user.LastName}",

                        Email =
                            user.Email ?? string.Empty,

                        UserName =
                            user.UserName ?? string.Empty,

                        IsActive =
                            user.IsActive,

                        Roles =
                            roles.ToList()
                    });
            }


            return View(model);
        }


        // =====================================================
        // AKTİF / PASİF
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(
            int id)
        {
            var user =
                await _userManager
                    .FindByIdAsync(
                        id.ToString());


            if (user == null)
            {
                return NotFound();
            }


            var currentUser =
                await _userManager
                    .GetUserAsync(User);


            // Admin kendi hesabını pasif yapamasın
            if (currentUser != null &&
                currentUser.Id == user.Id &&
                user.IsActive)
            {
                TempData["Error"] =
                    "Kendi hesabınızı pasif hale getiremezsiniz.";

                return RedirectToAction(
                    nameof(Users));
            }


            user.IsActive =
                !user.IsActive;


            var result =
                await _userManager
                    .UpdateAsync(user);


            if (!result.Succeeded)
            {
                TempData["Error"] =
                    "Kullanıcı durumu güncellenemedi.";

                return RedirectToAction(
                    nameof(Users));
            }


            TempData["Success"] =
                user.IsActive
                    ? "Kullanıcı aktif hale getirildi."
                    : "Kullanıcı pasif hale getirildi.";


            return RedirectToAction(
                nameof(Users));
        }


        // =====================================================
        // ADMIN ROLÜ VER
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(
            int id)
        {
            var user =
                await _userManager
                    .FindByIdAsync(
                        id.ToString());


            if (user == null)
            {
                return NotFound();
            }


            var isAdmin =
                await _userManager
                    .IsInRoleAsync(
                        user,
                        "Admin");


            if (!isAdmin)
            {
                var result =
                    await _userManager
                        .AddToRoleAsync(
                            user,
                            "Admin");


                if (!result.Succeeded)
                {
                    TempData["Error"] =
                        "Admin rolü verilemedi.";

                    return RedirectToAction(
                        nameof(Users));
                }
            }


            TempData["Success"] =
                $"{user.FirstName} {user.LastName} artık Admin.";


            return RedirectToAction(
                nameof(Users));
        }


        // =====================================================
        // ADMIN ROLÜNÜ KALDIR
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAdmin(
            int id)
        {
            var user =
                await _userManager
                    .FindByIdAsync(
                        id.ToString());


            if (user == null)
            {
                return NotFound();
            }


            var currentUser =
                await _userManager
                    .GetUserAsync(User);


            // Admin kendi Admin rolünü kaldıramasın
            if (currentUser != null &&
                currentUser.Id == user.Id)
            {
                TempData["Error"] =
                    "Kendi Admin rolünüzü kaldıramazsınız.";

                return RedirectToAction(
                    nameof(Users));
            }


            var isAdmin =
                await _userManager
                    .IsInRoleAsync(
                        user,
                        "Admin");


            if (isAdmin)
            {
                var result =
                    await _userManager
                        .RemoveFromRoleAsync(
                            user,
                            "Admin");


                if (!result.Succeeded)
                {
                    TempData["Error"] =
                        "Admin rolü kaldırılamadı.";

                    return RedirectToAction(
                        nameof(Users));
                }
            }


            TempData["Success"] =
                $"{user.FirstName} {user.LastName} kullanıcısının Admin rolü kaldırıldı.";


            return RedirectToAction(
                nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return RedirectToAction("Login", "Auth");

            ViewBag.fullName =
                $"{currentUser.FirstName} {currentUser.LastName}";

            var roles = await _roleManager.Roles
                .OrderBy(x => x.Name)
                .Select(x => new RoleDto
                {
                    Id = x.Id,
                    Name = x.Name ?? ""
                })
                .ToListAsync();

            var users = await _userManager.Users
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync();

            var userDtos = new List<UserRoleDto>();

            foreach (var user in users)
            {
                var userRoles =
                    await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserRoleDto
                {
                    Id = user.Id,

                    FullName =
                        $"{user.FirstName} {user.LastName}",

                    Email =
                        user.Email ?? "",

                    Roles =
                        userRoles.ToList()
                });
            }

            var model = new RoleManagementDto
            {
                Roles = roles,
                Users = userDtos
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["Error"] =
                    "Rol adı boş bırakılamaz.";

                return RedirectToAction(nameof(Roles));
            }

            roleName = roleName.Trim();

            var exists =
                await _roleManager.RoleExistsAsync(roleName);

            if (exists)
            {
                TempData["Error"] =
                    "Bu rol zaten mevcut.";

                return RedirectToAction(nameof(Roles));
            }

            var result =
                await _roleManager.CreateAsync(
                    new AppRole
                    {
                        Name = roleName
                    });

            if (!result.Succeeded)
            {
                TempData["Error"] =
                    "Rol oluşturulamadı.";

                return RedirectToAction(nameof(Roles));
            }

            TempData["Success"] =
                $"{roleName} rolü oluşturuldu.";

            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role =
                await _roleManager.FindByIdAsync(
                    id.ToString());

            if (role == null)
                return NotFound();

            if (role.Name == "Admin" ||
                role.Name == "User")
            {
                TempData["Error"] =
                    "Admin ve User rolleri silinemez.";

                return RedirectToAction(nameof(Roles));
            }

            var usersInRole =
                await _userManager
                    .GetUsersInRoleAsync(role.Name!);

            if (usersInRole.Any())
            {
                TempData["Error"] =
                    "Bu rol bazı kullanıcılara atanmış. Önce kullanıcılardan rolü kaldırın.";

                return RedirectToAction(nameof(Roles));
            }

            var result =
                await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                TempData["Error"] =
                    "Rol silinemedi.";

                return RedirectToAction(nameof(Roles));
            }

            TempData["Success"] =
                $"{role.Name} rolü silindi.";

            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserRole(
    int userId,
    string roleName)
        {
            var user =
                await _userManager.FindByIdAsync(
                    userId.ToString());

            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["Error"] =
                    "Rol seçmelisiniz.";

                return RedirectToAction(nameof(Roles));
            }

            var roleExists =
                await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                TempData["Error"] =
                    "Seçilen rol bulunamadı.";

                return RedirectToAction(nameof(Roles));
            }

            var alreadyInRole =
                await _userManager
                    .IsInRoleAsync(
                        user,
                        roleName);

            if (alreadyInRole)
            {
                TempData["Error"] =
                    "Kullanıcı zaten bu role sahip.";

                return RedirectToAction(nameof(Roles));
            }

            var result =
                await _userManager
                    .AddToRoleAsync(
                        user,
                        roleName);

            if (!result.Succeeded)
            {
                TempData["Error"] =
                    "Rol kullanıcıya atanamadı.";

                return RedirectToAction(nameof(Roles));
            }

            TempData["Success"] =
                $"{user.FirstName} {user.LastName} kullanıcısına {roleName} rolü verildi.";

            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserRole(
    int userId,
    string roleName)
        {
            var user =
                await _userManager.FindByIdAsync(
                    userId.ToString());

            if (user == null)
                return NotFound();

            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser != null &&
                currentUser.Id == user.Id &&
                roleName == "Admin")
            {
                TempData["Error"] =
                    "Kendi Admin rolünüzü kaldıramazsınız.";

                return RedirectToAction(nameof(Roles));
            }

            var isInRole =
                await _userManager
                    .IsInRoleAsync(
                        user,
                        roleName);

            if (!isInRole)
            {
                TempData["Error"] =
                    "Kullanıcı bu role sahip değil.";

                return RedirectToAction(nameof(Roles));
            }

            var result =
                await _userManager
                    .RemoveFromRoleAsync(
                        user,
                        roleName);

            if (!result.Succeeded)
            {
                TempData["Error"] =
                    "Rol kullanıcıdan kaldırılamadı.";

                return RedirectToAction(nameof(Roles));
            }

            TempData["Success"] =
                $"{roleName} rolü kullanıcıdan kaldırıldı.";

            return RedirectToAction(nameof(Roles));
        }

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return RedirectToAction("Login", "Auth");

            ViewBag.fullName =
                $"{currentUser.FirstName} {currentUser.LastName}";

            var reports = await _context.MessageReports
        .Include(x => x.Reporter)
        .Include(x => x.Message)
            .ThenInclude(x => x.Sender)
        .Include(x => x.Message)
            .ThenInclude(x => x.Receiver)
        .OrderBy(x => x.Status == ReportStatus.Pending ? 0 : 1)
        .ThenByDescending(x => x.CreatedDate)
        .ToListAsync();

            return View(reports);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReportStatus(
    int id,
    ReportStatus status)
        {
            var admin =
                await _userManager.GetUserAsync(User);

            if (admin == null)
                return Unauthorized();

            var report =
                await _context.MessageReports
                    .FirstOrDefaultAsync(x =>
                        x.Id == id);

            if (report == null)
                return NotFound();

            report.Status = status;
            report.ReviewedDate = DateTime.Now;
            report.ReviewedById = admin.Id;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Şikayet durumu güncellendi.";

            return RedirectToAction(nameof(Reports));
        }

        // ŞİFRE SIFIRLAMA TALEPLERİ
        [HttpGet]
        public async Task<IActionResult> PasswordResetRequests()
        {
            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return RedirectToAction("Login", "Auth");

            ViewBag.fullName =
                $"{currentUser.FirstName} {currentUser.LastName}";

            var requests =
                await _context.PasswordResetRequests
                    .Include(x => x.User)
                    .OrderBy(x => x.IsCompleted)
                    .ThenByDescending(x => x.RequestDate)
                    .ToListAsync();

            return View(requests);
        }

        [HttpGet]
        public async Task<IActionResult> ResetUserPassword(
    int id)
        {
            var request =
                await _context.PasswordResetRequests
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x =>
                        x.Id == id &&
                        !x.IsCompleted);

            if (request == null)
                return NotFound();

            ViewBag.UserFullName =
                $"{request.User.FirstName} {request.User.LastName}";

            ViewBag.UserEmail =
                request.User.Email;

            var model = new ResetUserPasswordDto
            {
                RequestId = request.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetUserPassword(
      ResetUserPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = await _context.PasswordResetRequests
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == model.RequestId);

            if (request == null)
            {
                TempData["Error"] =
                    "Şifre sıfırlama talebi bulunamadı.";

                return RedirectToAction(
                    nameof(PasswordResetRequests));
            }

            if (request.IsCompleted)
            {
                TempData["Error"] =
                    "Bu şifre sıfırlama talebi daha önce tamamlanmış.";

                return RedirectToAction(
                    nameof(PasswordResetRequests));
            }

            var user = request.User;

            if (user == null)
            {
                TempData["Error"] =
                    "Talebe ait kullanıcı bulunamadı.";

                return RedirectToAction(
                    nameof(PasswordResetRequests));
            }


            // Identity'nin kendi password reset token'ını üret
            var token =
                await _userManager
                    .GeneratePasswordResetTokenAsync(user);


            // Yeni şifreyi uygula
            var result =
                await _userManager.ResetPasswordAsync(
                    user,
                    token,
                    model.NewPassword);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                ViewBag.UserFullName =
                    $"{user.FirstName} {user.LastName}";

                ViewBag.UserEmail =
                    user.Email;

                return View(model);
            }


            // Talebi tamamlandı olarak işaretle
            request.IsCompleted = true;

            // Entity'de böyle bir alanın varsa:
            // request.CompletedDate = DateTime.Now;

            await _context.SaveChangesAsync();


            TempData["Success"] =
                $"{user.FirstName} {user.LastName} kullanıcısının şifresi başarıyla sıfırlandı.";


            return RedirectToAction(
                nameof(PasswordResetRequests));
        }
    }
}