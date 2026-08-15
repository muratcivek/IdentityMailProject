using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.UserDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers
{

    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Message");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var existingUser =
                await _userManager.FindByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    nameof(registerDto.Email),
                    "Bu e-posta adresi zaten kullanılıyor.");

                return View(registerDto);
            }

            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                IsActive = true
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    registerDto.Password);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(registerDto);
            }

            await _userManager.AddToRoleAsync(
                user,
                "User");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Message");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var user =
                await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "E-posta veya şifre hatalı.");

                return View(loginDto);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Hesabınız pasif durumda. Yönetici ile iletişime geçiniz.");

                return View(loginDto);
            }

            var result =
                await _signInManager.PasswordSignInAsync(
                    user,
                    loginDto.Password,
                    isPersistent: true,
                    lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "E-posta veya şifre hatalı.");

                return View(loginDto);
            }

            return RedirectToAction("Index", "Message");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            if (User.Identity != null &&
                User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Message");
            }

            return View(new ForgotPasswordDto());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(
    ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user =
                await _userManager.FindByEmailAsync(model.Email);

            // Güvenlik nedeniyle kullanıcı bulunup bulunmadığını
            // dışarıya söylemiyoruz.
            if (user != null)
            {
                var existingRequest =
                    await _context.PasswordResetRequests
                        .AnyAsync(x =>
                            x.UserId == user.Id &&
                            !x.IsCompleted);

                // Aynı kullanıcı için açık bir talep varsa
                // tekrar oluşturma.
                if (!existingRequest)
                {
                    var request =
                        new PasswordResetRequest
                        {
                            UserId = user.Id,
                            RequestDate = DateTime.Now,
                            IsCompleted = false
                        };

                    _context.PasswordResetRequests.Add(request);

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(
                nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}