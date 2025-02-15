using Karakatsiya.Interfaces;

namespace Karakatsiya.Services
{
    public class ConfirmationCodeGenerator : IConfirmationCodeGenerator
    {
        public string GenerateCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }
    }
}
