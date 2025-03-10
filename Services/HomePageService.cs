using Karakatsiya.Data;
using Karakatsiya.Interfaces;
using Karakatsiya.Models.PageSettings;
using Microsoft.EntityFrameworkCore;

namespace Karakatsiya.Services
{
    public class HomePageService : IHomePageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomePageService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ContactInfo?> GetContactInfoAsync()
        {
            return await _context.ContactInfos.FirstOrDefaultAsync() ?? new ContactInfo();
        }

        public async Task UpdateContactInfoAsync(ContactInfo contactInfo)
        {
            var existingContact = await _context.ContactInfos.FirstOrDefaultAsync();
            if (existingContact == null)
            {
                await _context.ContactInfos.AddAsync(contactInfo);
            }
            else
            {
                existingContact.Address = contactInfo.Address;
                existingContact.Phone = contactInfo.Phone;
                existingContact.Email = contactInfo.Email;
                _context.ContactInfos.Update(existingContact);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsAsync()
        {
            return await _context.NewsArticles.OrderByDescending(n => n.CreationDate).ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsByIdAsync(int id)
        {
            return await _context.NewsArticles.FindAsync(id);
        }

        public async Task AddNewsAsync(NewsArticle newsArticle, IFormFile? image)
        {
            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                newsArticle.ImageUrl = $"/images/{fileName}";
            }

            await _context.NewsArticles.AddAsync(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewsAsync(NewsArticle newsArticle, IFormFile? image)
        {
            var existingNews = await _context.NewsArticles.FindAsync(newsArticle.Id);
            if (existingNews != null)
            {
                existingNews.Title = newsArticle.Title;
                existingNews.Summary = newsArticle.Summary;

                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    existingNews.ImageUrl = $"/images/{fileName}";
                }

                _context.NewsArticles.Update(existingNews);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteNewsAsync(int id)
        {
            var newsArticle = await _context.NewsArticles.FindAsync(id);
            if (newsArticle != null)
            {
                _context.NewsArticles.Remove(newsArticle);
                await _context.SaveChangesAsync();
            }
        }
    }
}