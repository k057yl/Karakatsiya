using Karakatsiya.Services;
using Karakatsiya.Models.PageSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Karakatsiya.Interfaces;

namespace Karakatsiya.Controllers
{
    public class HomeController : BaseController
    {
        private readonly CurrencyService _currencyService;
        private readonly IHomePageService _homePageService;

        public HomeController(IStringLocalizer<HomeController> localizer, CurrencyService currencyService, IHomePageService homePageService) : base()
        {
            _currencyService = currencyService;
            _homePageService = homePageService;
        }

        public async Task<IActionResult> Index()
        {
            var exchangeRates = await _currencyService.GetExchangeRatesAsync();
            ViewData["ExchangeRates"] = exchangeRates;

            var contactInfo = await _homePageService.GetContactInfoAsync();

            var newsArticles = await _homePageService.GetNewsAsync();

            var model = (contactInfo, newsArticles);

            return View(model);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}