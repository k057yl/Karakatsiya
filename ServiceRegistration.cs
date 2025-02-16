using Karakatsiya.Localizations;
using Karakatsiya.Services;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<EmailService>();
        services.AddScoped<ConfirmationCodeGenerator>();
        services.AddScoped<HtmlValidator>();
        services.AddScoped<SharedLocalizationService>();
    }
}
