using Karakatsiya.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    public class NewsController : BaseController
    {
        private readonly IHomePageService _homePageService;

        public NewsController(IHomePageService homePageService)
        {
            _homePageService = homePageService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var newsArticles = await _homePageService.GetNewsAsync();
            var news = newsArticles.FirstOrDefault(n => n.Id == id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
    }
}
