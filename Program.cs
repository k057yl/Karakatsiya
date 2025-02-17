using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Karakatsiya.Models.Entities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Karakatsiya.Data;

var builder = WebApplication.CreateBuilder(args);

// ��������� ���� ������ PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� Identity � �������������� ����������������� ������ ApplicationUser
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ��������� �����������
builder.Services.AddLocalization(options => options.ResourcesPath = "Localizations");
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// ��������� ��������� ���������� ������
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("uk-UA") };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

// ���������� �������� ��� ������ � �������� � ������������
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // ������������� ����� ����� ������
    options.Cookie.HttpOnly = true; // �������� ������ � ������
    options.Cookie.IsEssential = true; // ���� ����������� ��� ������
});

// ����������� ��������� ��������
builder.Services.AddApplicationServices();

// ���������� MVC � Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ���������� ����������� � ������
app.UseRequestLocalization();
app.UseSession();

// ������������ pipeline ��� ��������� ������ � ������ �����
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // �������������� ������������� HTTPS
}

app.UseHttpsRedirection(); // ��������������� �� HTTPS
app.UseStaticFiles(); // ������������ ����������� ������
app.UseRouting(); // ������������� ��������

// ��������� �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

// ��������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // ������� ��� ������������

app.MapRazorPages(); // �������� ��� Razor Pages

app.Run();