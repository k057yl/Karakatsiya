using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Karakatsiya.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IStringLocalizer<HomeController> localizer) : base()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
