using Karakatsiya.Interfaces;
using Karakatsiya.Models.PageSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    [Authorize(Roles = "Gala")]
    public class AdminController : BaseController
    {
        private readonly IHomePageSettingsService _service;

        public AdminController(IHomePageSettingsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _service.GetSettingsAsync();
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Save(HomePageSettings model, IFormFile Image1File, IFormFile Image2File, IFormFile Image3File)
        {
            var settings = await _service.GetSettingsAsync();

            _service.SaveContacts(model, settings);
            _service.SaveNews(model, settings);

            await _service.SaveImage(Image1File, "Image1", settings);
            await _service.SaveImage(Image2File, "Image2", settings);
            await _service.SaveImage(Image3File, "Image3", settings);

            await _service.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
