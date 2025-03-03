using Karakatsiya.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Karakatsiya.Controllers
{
    public class HomeController : BaseController
    {
        private readonly CurrencyService _currencyService;

        public HomeController(IStringLocalizer<HomeController> localizer, CurrencyService currencyService) : base()
        {
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Index()
        {
            var exchangeRates = await _currencyService.GetExchangeRatesAsync();
            ViewData["ExchangeRates"] = exchangeRates;

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
