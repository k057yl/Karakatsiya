using Karakatsiya.Services;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.ViewComponents
{
    public class CurrencyRatesViewComponent : ViewComponent
    {
        private readonly CurrencyService _currencyService;

        public CurrencyRatesViewComponent(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rates = await _currencyService.GetExchangeRatesAsync();
            return View(rates);
        }
    }
}
