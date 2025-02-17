using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Karakatsiya.Models.Entities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Karakatsiya.Data;

var builder = WebApplication.CreateBuilder(args);

// Настройка базы данных PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка Identity с использованием пользовательского класса ApplicationUser
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Настройка локализации
builder.Services.AddLocalization(options => options.ResourcesPath = "Localizations");
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// Настройка поддержки нескольких языков
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("uk-UA") };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

// Добавление сервисов для работы с сессиями и кэшированием
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Устанавливаем время жизни сессии
    options.Cookie.HttpOnly = true; // Ожидание работы с куками
    options.Cookie.IsEssential = true; // Куки обязательны для работы
});

// Подключение кастомных сервисов
builder.Services.AddApplicationServices();

// Добавление MVC и Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Применение локализации и сессий
app.UseRequestLocalization();
app.UseSession();

// Конфигурация pipeline для обработки ошибок и других задач
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Принудительное использование HTTPS
}

app.UseHttpsRedirection(); // Перенаправление на HTTPS
app.UseStaticFiles(); // Обслуживание статических файлов
app.UseRouting(); // Маршрутизация запросов

// Настройка аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Маршруты
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Роутинг для контроллеров

app.MapRazorPages(); // Маршруты для Razor Pages

app.Run();