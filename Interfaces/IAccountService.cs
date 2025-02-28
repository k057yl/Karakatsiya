using Karakatsiya.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Karakatsiya.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityUser> RegisterUserAsync(RegisterDto model);
        Task<bool> ConfirmEmailAsync(string email, string confirmationCode);
        Task ResendConfirmationCodeAsync(string email);
        Task<SignInResult> LoginUserAsync(LoginDto model);
        Task LogoutUserAsync();
    }
}
