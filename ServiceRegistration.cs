using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Services;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Singleton
        services.AddSingleton<SharedLocalizationService>();
        services.AddSingleton<CategoryLocalizationService>();
        services.AddSingleton<CurrencyLocalizationService>();
        services.AddHttpClient<CurrencyService>();

        //Transient
        services.AddTransient<EmailService>();
        services.AddTransient<HtmlValidator>();
        services.AddTransient<ConfirmationCodeGenerator>();

        //Scoped
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IHomePageService, HomePageService>();
        services.AddScoped<IItemValidationService, ItemValidationService>();

        services.AddHttpContextAccessor();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }
}