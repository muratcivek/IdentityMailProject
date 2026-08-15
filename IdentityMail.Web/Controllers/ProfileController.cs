using IdentityMail.Web.DTOs.UserDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new ProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImageUrl = user.ProfileImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (model.ProfileImage != null)
            {
                var allowedExtensions = new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };

                var extension = Path
                    .GetExtension(model.ProfileImage.FileName)
                    .ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        "ProfileImage",
                        "Sadece jpg, jpeg, png veya webp dosyaları yüklenebilir."
                    );

                    model.ProfileImageUrl = user.ProfileImageUrl;

                    return View(model);
                }

                var fileName = Guid.NewGuid().ToString() + extension;

                var folder = Path.Combine(
                    _environment.WebRootPath,
                    "profile-images"
                );

                Directory.CreateDirectory(folder);

                var filePath = Path.Combine(
                    folder,
                    fileName
                );

                await using var stream = new FileStream(
                    filePath,
                    FileMode.Create
                );

                await model.ProfileImage.CopyToAsync(stream);

                user.ProfileImageUrl =
                    "/profile-images/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                model.ProfileImageUrl = user.ProfileImageUrl;

                return View(model);
            }

            TempData["Success"] =
                "Profiliniz başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth"
                );
            }

            var result =
                await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword
                );

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return View(model);
            }

            // Şifre değiştiği için kullanıcının giriş cookie'sini yeniler.
            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] =
                "Şifreniz başarıyla değiştirildi.";

            return RedirectToAction(
                nameof(ChangePassword)
            );
        }
    }
}