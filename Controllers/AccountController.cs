using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly SharedLocalizationService _localizationService;

        public AccountController(IAccountService accountService, SharedLocalizationService sharedLocalizationService)
        {
            _accountService = accountService;
            _localizationService = sharedLocalizationService;
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
                    TempData["Message"] = _localizationService.Messages["SendingAnEmail"].Value;
                    return RedirectToAction(ProjectConstants.PAGE_CONFIRM_EMAIL, new { email = model.Email });
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
                    return RedirectToAction(ProjectConstants.PAGE_INDEX, ProjectConstants.CONTROLLER_HOME);
                }

                ModelState.AddModelError(string.Empty, _localizationService.WarningMessages["IncorrectLoginOrPassword"].Value);
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
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto model)
        {
            if (await _accountService.ConfirmEmailAsync(model.Email, model.ConfirmationCode))
            {
                return RedirectToAction(ProjectConstants.PAGE_LOGIN);
            }

            ModelState.AddModelError(string.Empty, _localizationService.WarningMessages["InvalidVerificationCode"].Value);
            return View(model);
        }
        /*
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string email, string confirmationCode)
        {
            if (await _accountService.ConfirmEmailAsync(email, confirmationCode))
            {
                return RedirectToAction(ProjectConstants.PAGE_LOGIN);
            }

            ModelState.AddModelError(string.Empty, _localizationService.WarningMessages["InvalidVerificationCode"].Value);
            return View();
        }
        */
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUserAsync();
            return RedirectToAction(ProjectConstants.PAGE_LOGIN, ProjectConstants.CONTROLLER_ACCOUNT);
        }

        public IActionResult NotAccess() => View();

        [HttpPost]
        public async Task<IActionResult> ResendCode()
        {
            try
            {
                await _accountService.ResendConfirmationCodeAsync(User.Identity.Name);
                TempData["Message"] = _localizationService.Messages["SendingAnEmail"].Value;
                TempData["ResendTimer"] = ProjectConstants.RESEND_CODE_TIMER_SECONDS;
            }
            catch (Exception ex)
            {
                TempData["Warning"] = ex.Message;
                TempData["ResendTimer"] = ProjectConstants.RESEND_CODE_TIMER_SECONDS;
            }
            return RedirectToAction(ProjectConstants.PAGE_CONFIRM_EMAIL);
        }
    }
}