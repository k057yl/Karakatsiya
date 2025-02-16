using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Karakatsiya.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(IStringLocalizer<HomeController> localizer) : base(localizer)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
