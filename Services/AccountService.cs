using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Karakatsiya.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ConfirmationCodeGenerator _codeGenerator;
        private readonly SharedLocalizationService _localization;

        public AccountService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            EmailService emailService,
            ConfirmationCodeGenerator codeGenerator,
            SharedLocalizationService localization)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _codeGenerator = codeGenerator;
            _localization = localization;
        }

        public async Task<IdentityUser> RegisterUserAsync(RegisterDto model)
        {
            if (string.IsNullOrEmpty(model.Email) || !Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException(_localization.WarningMessages["InvalidEmailFormat"]);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentException("Password cannot be empty");
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var confirmationCode = _codeGenerator.GenerateCode();
                var subject = _localization.Messages["RegistrationConfirmationCode"];
                var body = $"{_localization.Messages["YourVerificationCode"]} {confirmationCode}";
                await _emailService.SendEmailAsync(model.Email, subject, body);
                return user;
            }

            throw new InvalidOperationException(string.Join(",", result.Errors.Select(e => e.Description)));
        }

        public async Task<bool> ConfirmEmailAsync(string email, string confirmationCode)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(confirmationCode))
            {
                throw new ArgumentException("Email and confirmation code cannot be empty.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new ArgumentException("Пользователь не найден.");

            var storedCode = confirmationCode;
            if (storedCode == confirmationCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return true;
            }

            return false;
        }

        public async Task<SignInResult> LoginUserAsync(LoginDto model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentException("Email and password cannot be empty.");
            }

            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}