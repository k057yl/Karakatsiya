using Karakatsiya.Localizations;
using Karakatsiya.Models.Entities;

namespace Karakatsiya.Services
{
    public class CategoryLocalizationService
    {
        private readonly SharedLocalizationService _localizer;

        public CategoryLocalizationService(SharedLocalizationService localizer)
        {
            _localizer = localizer;
        }

        public IDictionary<Category, string> GetLocalizedCategories()
        {
            return Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .ToDictionary(
                    category => category,
                    category => _localizer.Categories[category.ToString()]?.Value ?? category.ToString());
        }
    }
}
