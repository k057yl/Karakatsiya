using Karakatsiya.Models.PageSettings;

namespace Karakatsiya.Interfaces
{
    public interface IHomePageSettingsService
    {
        Task<HomePageSettings> GetSettingsAsync();
        void SaveContacts(HomePageSettings model, HomePageSettings settings);
        void SaveNews(HomePageSettings model, HomePageSettings settings);
        Task SaveImage(IFormFile file, string imageProperty, HomePageSettings settings);
        Task SaveChangesAsync();
    }
}
