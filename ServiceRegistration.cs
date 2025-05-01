using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Services;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Изменить на Scoped, так как эти сервисы зависят от ApplicationDbContext
        services.AddScoped<SharedLocalizationService>();
        services.AddScoped<CategoryLocalizationService>();
        services.AddScoped<CurrencyLocalizationService>();

        // Регистрируем HttpClient
        services.AddHttpClient<CurrencyService>();

        // Transient сервисы
        services.AddTransient<EmailService>();
        services.AddTransient<HtmlValidator>();
        services.AddTransient<ConfirmationCodeGenerator>();

        // Scoped сервисы
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IHomePageService, HomePageService>();
        services.AddScoped<IItemValidationService, ItemValidationService>();

        // Добавляем HttpContextAccessor
        services.AddHttpContextAccessor();

        // Настройки для сессий
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }
}