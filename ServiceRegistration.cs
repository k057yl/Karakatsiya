using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Services;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
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

        services.AddHttpContextAccessor();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddScoped<IHomePageService, HomePageService>();

        services.AddHttpClient<CurrencyService>();
        services.AddScoped<CurrencyService>();
    }
}