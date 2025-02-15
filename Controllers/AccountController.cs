using Karakatsiya.Models.DTOs;
using Karakatsiya.Models.Entities;
using Karakatsiya.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService;
        private readonly ConfirmationCodeGenerator _codeGenerator;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            EmailService emailService,
            ConfirmationCodeGenerator codeGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _codeGenerator = codeGenerator;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var confirmationCode = _codeGenerator.GenerateCode();
                    user.ConfirmationCode = confirmationCode;
                    await _userManager.UpdateAsync(user);

                    var subject = "Код подтверждения регистрации";
                    var body = $"Ваш код подтверждения: {confirmationCode}";
                    await _emailService.SendEmailAsync(model.Email, subject, body);

                    return RedirectToAction("ConfirmEmail", new { email = model.Email });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неправильный логин или пароль.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string email, string confirmationCode)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден.");
                return View();
            }

            if (user.ConfirmationCode == confirmationCode)
            {
                user.EmailConfirmed = true;
                user.ConfirmationCode = null;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Неверный код подтверждения.");
            return View();
        }
    }
}