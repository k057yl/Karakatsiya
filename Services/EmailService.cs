using brevo_csharp.Api;
using brevo_csharp.Client;
using brevo_csharp.Model;
using Task = System.Threading.Tasks.Task;

namespace Karakatsiya.Services
{
    public class EmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["Brevo:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey));
            _fromEmail = configuration["Brevo:FromEmail"] ?? throw new ArgumentNullException(nameof(_fromEmail));
            _fromName = configuration["Brevo:FromName"] ?? throw new ArgumentNullException(nameof(_fromName));
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var apiInstance = new TransactionalEmailsApi();
            Configuration.Default.ApiKey.Add("api-key", _apiKey);

            var sendSmtpEmail = new SendSmtpEmail(
                sender: new SendSmtpEmailSender(_fromName, _fromEmail),
                to: new List<SendSmtpEmailTo> { new SendSmtpEmailTo(toEmail) },
                subject: subject,
                htmlContent: htmlContent
            );

            try
            {
                var result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending email: " + e.Message);
            }
        }
    }
}
