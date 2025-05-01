using Karakatsiya.Data;
using Karakatsiya.Localizations;
using Microsoft.EntityFrameworkCore;

namespace Karakatsiya.Services
{
    public class CategoryLocalizationService
    {
        private readonly SharedLocalizationService _localizer;
        private readonly ApplicationDbContext _context;

        public CategoryLocalizationService(SharedLocalizationService localizer, ApplicationDbContext context)
        {
            _localizer = localizer;
            _context = context;
        }

        public async Task<Dictionary<int, string>> GetLocalizedCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories.ToDictionary(
                c => c.Id,
                c => _localizer.Categories[c.ResourceKey] ?? c.Name
            );
        }
    }
}