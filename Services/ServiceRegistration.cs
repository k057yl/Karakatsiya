namespace Karakatsiya.Services
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<EmailService>();
            services.AddScoped<ConfirmationCodeGenerator>();
        }
    }
}
