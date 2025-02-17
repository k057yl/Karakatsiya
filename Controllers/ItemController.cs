using Karakatsiya.Interfaces;
using Karakatsiya.Models.DTOs;
using Karakatsiya.Models.Entities;
using Karakatsiya.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    [Authorize]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly UserManager<ApplicationUser> _userManager;//IdentityUser
        private readonly HtmlValidator _htmlValidator;

        public ItemController(
            IItemService itemService,
            UserManager<ApplicationUser> userManager,
            HtmlValidator htmlValidator)
            : base()
        {
            _itemService = itemService;
            _htmlValidator = htmlValidator;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateItem() => View();

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemDto model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден.");
                return View(model);
            }

            var item = await _itemService.CreateItemAsync(model, user.Id);
            return RedirectToAction("UserItemsList");
        }

        [HttpGet]
        public async Task<IActionResult> UserItems()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var items = await _itemService.GetUserItemsAsync(user.Id);
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !await _itemService.DeleteItemAsync(id, user.Id))
            {
                return NotFound();
            }

            return RedirectToAction("UserItemsList");
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemService.GetItemDetailsAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var item = await _itemService.GetItemDetailsAsync(id);
            if (item == null || item.UserId != user.Id)
            {
                return NotFound();
            }

            var model = new CreateItemDto
            {
                Name = item.Name,
                Description = item.Description,
                ExpirationDate = item.ExpirationDate,
                Category = item.Category,
                Price = item.Price,
                Currency = item.Currency,
                ExistingImagePath = item.ImagePath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditItemDto model, string captcha)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var item = await _itemService.EditItemAsync(id, model, user.Id);
            if (item == null)
            {
                return NotFound();
            }

            return RedirectToAction("UserItemsList");
        }
    }
}
