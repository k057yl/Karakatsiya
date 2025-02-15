using Microsoft.AspNetCore.Identity;

namespace Karakatsiya.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? ConfirmationCode { get; set; }
    }
}
