using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Karakatsiya.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ConfirmationCodeGenerator _codeGenerator;
        private readonly SharedLocalizationService _localization;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AccountService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            EmailService emailService,
            ConfirmationCodeGenerator codeGenerator,
            SharedLocalizationService localization,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _codeGenerator = codeGenerator;
            _localization = localization;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<IdentityUser> RegisterUserAsync(RegisterDto model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var confirmationCode = _codeGenerator.GenerateCode();
                _httpContextAccessor.HttpContext.Session.SetString("ConfirmationCode", confirmationCode);
                _httpContextAccessor.HttpContext.Session.SetString("CodeExpiration", DateTime.UtcNow.AddMinutes(10).ToString());

                var subject = _localization.Messages["RegistrationConfirmationCode"];
                var body = $"{_localization.Messages["YourVerificationCode"]} {confirmationCode}";

                await _emailService.SendEmailAsync(model.Email, subject, body);

                var adminEmails = _configuration.GetSection("AdminEmails").Get<string[]>();
                if (adminEmails != null && adminEmails.Contains(model.Email))
                {
                    await _userManager.AddToRoleAsync(user, "Gala");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "regularuser");
                }
            }

            return user;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string code)
        {
            var storedCode = _httpContextAccessor.HttpContext.Session.GetString("ConfirmationCode");
            var expiration = _httpContextAccessor.HttpContext.Session.GetString("CodeExpiration");

            if (storedCode == null || expiration == null)
            {
                throw new InvalidOperationException("Код не найден или срок действия истек.");
            }

            if (DateTime.UtcNow > DateTime.Parse(expiration))
            {
                throw new InvalidOperationException("Срок действия кода истек.");
            }

            if (storedCode == code)
            {
                var user = await _userManager.FindByEmailAsync(email);
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                _httpContextAccessor.HttpContext.Session.Remove("ConfirmationCode");
                _httpContextAccessor.HttpContext.Session.Remove("CodeExpiration");
                return true;
            }

            throw new ArgumentException("Неверный код.");
        }

        public async Task ResendConfirmationCodeAsync(string email)
        {
            var lastSent = _httpContextAccessor.HttpContext.Session.GetString("LastSent");
            if (lastSent != null && DateTime.UtcNow < DateTime.Parse(lastSent).AddMinutes(1))
            {
                throw new Exception(_localization.Messages["WaitOneMinute"]);
            }

            var confirmationCode = _codeGenerator.GenerateCode();
            _httpContextAccessor.HttpContext.Session.SetString("ConfirmationCode", confirmationCode);
            _httpContextAccessor.HttpContext.Session.SetString("CodeExpiration", DateTime.UtcNow.AddMinutes(10).ToString());
            _httpContextAccessor.HttpContext.Session.SetString("LastSent", DateTime.UtcNow.ToString());

            var subject = _localization.Messages["RegistrationConfirmationCode"];
            var body = $"{_localization.Messages["YourVerificationCode"]} {confirmationCode}";

            await _emailService.SendEmailAsync(email, subject, body);
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