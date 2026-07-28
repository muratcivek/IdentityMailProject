using IdentityMail.Web.DTOs.UserDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    public class AuthController(UserManager<AppUser> _usertManager,
                                            SignInManager<AppUser> _signInManager
                                            ) : Controller
    {
        //private readonly UserManager<AppUser> _userManager;

        //public AuthController(UserManager<AppUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Şifreler uyuşmuyor.");
                return View(registerDto);
            }
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };
            var result = await _usertManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerDto);
            }
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
            var user = await _usertManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginDto);
            }

            var result = await _usertManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginDto);
            }

            var resultSignIn = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            return RedirectToAction("Index", "Message");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
