using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Services;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<EmailService>();
        services.AddScoped<ConfirmationCodeGenerator>();
        services.AddScoped<HtmlValidator>();
        services.AddScoped<SharedLocalizationService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<CategoryLocalizationService>();
        services.AddScoped<CurrencyLocalizationService>();
    }
}
