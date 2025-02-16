using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    public class BaseController : Controller
    {
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (string.IsNullOrEmpty(culture))
            {
                culture = "en-US";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }

            return LocalRedirect(returnUrl);
        }
    }
}
