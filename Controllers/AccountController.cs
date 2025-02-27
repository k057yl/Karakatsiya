using Karakatsiya.Interfaces;
using Karakatsiya.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _accountService.RegisterUserAsync(model);
                    TempData["Message"] = "Письмо с кодом подтверждения отправлено на ваш Email.";
                    return RedirectToAction("ConfirmEmail", new { email = model.Email });
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(nameof(model.Email), ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginUserAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Неправильный логин или пароль.");
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
            if (await _accountService.ConfirmEmailAsync(email, confirmationCode))
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Неверный код подтверждения.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUserAsync();
            return RedirectToAction("Login", "Account");
        }

        //[Authorize(Roles = "Gala")]//********
        public IActionResult AdminPanel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendCode()//************
        {
            try
            {
                await _accountService.ResendConfirmationCodeAsync(User.Identity.Name);
                TempData["Message"] = "CodeSentAgain";
                TempData["ResendTimer"] = 60;
            }
            catch (Exception ex)
            {
                TempData["Warning"] = ex.Message;
                TempData["ResendTimer"] = 60;
            }
            return RedirectToAction("ConfirmEmail");
        }
    }
}