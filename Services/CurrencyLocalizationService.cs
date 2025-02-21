using Karakatsiya.Localizations;
using Karakatsiya.Models.Entities;

namespace Karakatsiya.Services
{
    public class CurrencyLocalizationService
    {
        private readonly SharedLocalizationService _localizer;

        public CurrencyLocalizationService(SharedLocalizationService localizer)
        {
            _localizer = localizer;
        }

        public IDictionary<Currency, string> GetLocalizedCurrencies()
        {
            return Enum.GetValues(typeof(Currency))
                .Cast<Currency>()
                .ToDictionary(
                    currency => currency,
                    currency => _localizer.Currencies[currency.ToString()]?.Value ?? currency.ToString());
        }
    }
}
