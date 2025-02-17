﻿using Karakatsiya.Localizations;
using Karakatsiya.Models.DTOs;
using Karakatsiya.Models.Entities;
using Karakatsiya.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Karakatsiya.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailService _emailService;
        private readonly ConfirmationCodeGenerator _codeGenerator;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly HtmlValidator _htmlValidator;
        private readonly SharedLocalizationService _localization;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            EmailService emailService,
            ConfirmationCodeGenerator codeGenerator,
            HtmlValidator htmlValidator,
            SharedLocalizationService localizer) : base()
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _codeGenerator = codeGenerator;
            _htmlValidator = htmlValidator;
            _localization = localizer;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ModelState.AddModelError(nameof(model.Email), _localization.WarningMessages["InvalidEmailFormat"]);
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var confirmationCode = _codeGenerator.GenerateCode();
                    TempData["ConfirmationCode"] = confirmationCode;

                    var subject = _localization.Messages["RegistrationConfirmationCode"];
                    var body = $"{_localization.Messages["YourVerificationCode"]} {confirmationCode}";
                    await _emailService.SendEmailAsync(model.Email, subject, body);

                    TempData["Message"] = "Письмо с кодом подтверждения отправлено на ваш Email.";
                    return RedirectToAction("ConfirmEmail", new { email = model.Email });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
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

            var storedCode = TempData["ConfirmationCode"]?.ToString();

            if (storedCode == confirmationCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Неверный код подтверждения.");
            return View();
        }
    }
}