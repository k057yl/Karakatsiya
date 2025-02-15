using System.ComponentModel.DataAnnotations;

namespace Karakatsiya.Models.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Введите корректный Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
