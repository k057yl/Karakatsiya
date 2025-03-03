using Karakatsiya.Data;
using Karakatsiya.Interfaces;
using Karakatsiya.Models.PageSettings;
using Microsoft.EntityFrameworkCore;

namespace Karakatsiya.Services
{
    public class HomePageSettingsService : IHomePageSettingsService
    {
        private readonly ApplicationDbContext _context;

        public HomePageSettingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<HomePageSettings> GetSettingsAsync()
        {
            return await _context.HomePageSettings.FirstOrDefaultAsync() ?? new HomePageSettings();
        }

        public void SaveContacts(HomePageSettings model, HomePageSettings settings)
        {
            settings.Address = string.IsNullOrEmpty(model.Address) ? settings.Address : model.Address;
            settings.Phone = string.IsNullOrEmpty(model.Phone) ? settings.Phone : model.Phone;
            settings.Email = string.IsNullOrEmpty(model.Email) ? settings.Email : model.Email;
        }

        public void SaveNews(HomePageSettings model, HomePageSettings settings)
        {
            settings.NewsTitle1 = string.IsNullOrEmpty(model.NewsTitle1) ? settings.NewsTitle1 : model.NewsTitle1;
            settings.NewsSummary1 = string.IsNullOrEmpty(model.NewsSummary1) ? settings.NewsSummary1 : model.NewsSummary1;
            settings.NewsDate1 = model.NewsDate1 == default ? settings.NewsDate1 : model.NewsDate1;
        }

        public async Task SaveImage(IFormFile file, string imageProperty, HomePageSettings settings)
        {
            if (file != null)
            {
                string path = Path.Combine("wwwroot/images", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                switch (imageProperty)
                {
                    case "Image1":
                        settings.Image1 = "/images/" + file.FileName;
                        break;
                    case "Image2":
                        settings.Image2 = "/images/" + file.FileName;
                        break;
                    case "Image3":
                        settings.Image3 = "/images/" + file.FileName;
                        break;
                }
            }
        }
    }
}
