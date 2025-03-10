using Karakatsiya.Models.PageSettings;

namespace Karakatsiya.Interfaces
{
    public interface IHomePageService
    {
        Task<ContactInfo?> GetContactInfoAsync();
        Task UpdateContactInfoAsync(ContactInfo contactInfo);

        Task<IEnumerable<NewsArticle>> GetNewsAsync();
        Task<NewsArticle?> GetNewsByIdAsync(int id);
        Task AddNewsAsync(NewsArticle newsArticle, IFormFile? image);
        Task UpdateNewsAsync(NewsArticle newsArticle, IFormFile? image);
        Task DeleteNewsAsync(int id);
    }
}
