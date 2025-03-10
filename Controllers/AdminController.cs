using Karakatsiya.Interfaces;
using Karakatsiya.Models.PageSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    [Authorize(Roles = "Gala")]
    public class AdminController : BaseController
    {
        private readonly IHomePageService _homePageService;

        public AdminController(IHomePageService homePageService)
        {
            _homePageService = homePageService;
        }

        public async Task<IActionResult> AdminPanel()
        {
            var contactInfo = await _homePageService.GetContactInfoAsync() ?? new ContactInfo();
            var newsList = await _homePageService.GetNewsAsync() ?? new List<NewsArticle>();

            return View((contactInfo, newsList));
        }

        public async Task<IActionResult> EditContactInfo()
        {
            var contactInfo = await _homePageService.GetContactInfoAsync() ?? new ContactInfo();
            return View(contactInfo);
        }

        [HttpPost]
        public async Task<IActionResult> EditContactInfo(ContactInfo model)
        {
            if (!ModelState.IsValid) return View(model);

            await _homePageService.UpdateContactInfoAsync(model);
            return RedirectToAction(nameof(AdminPanel));
        }

        public IActionResult CreateNews() => View(new NewsArticle());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNews(NewsArticle newsArticle, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                await _homePageService.AddNewsAsync(newsArticle, image);
                return RedirectToAction(nameof(AdminPanel));
            }
            return View(newsArticle);
        }

        public async Task<IActionResult> EditNews(int id)
        {
            var news = await _homePageService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNews(int id, NewsArticle newsArticle, IFormFile image)
        {
            if (id != newsArticle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _homePageService.UpdateNewsAsync(newsArticle, image);
                return RedirectToAction(nameof(AdminPanel));
            }

            return View(newsArticle);
        }

        public async Task<IActionResult> DeleteNews(int id)
        {
            await _homePageService.DeleteNewsAsync(id);
            return RedirectToAction(nameof(AdminPanel));
        }
    }
}
