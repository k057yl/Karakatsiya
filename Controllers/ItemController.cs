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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IItemValidationService _itemValidationService;

        private readonly CategoryLocalizationService _categoryLocalizationService;//************

        public ItemController(
            IItemService itemService,
            UserManager<IdentityUser> userManager,
            IItemValidationService itemValidationService,
            CategoryLocalizationService categoryLocalizationService)
            : base()
        {
            _itemService = itemService;
            _userManager = userManager;
            _itemValidationService = itemValidationService;
            _categoryLocalizationService = categoryLocalizationService;
        }

        [HttpGet]
        public IActionResult CreateItem()
        {
            return View(new CreateItemDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemDto model)
        {
            if (!ModelState.IsValid || !_itemValidationService.Validate(model, ModelState))
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View(model);
            }

            var imagePaths = new List<string>();
            string mainImage = model.MainImage;

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploads);

                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var relativePath = $"/images/{fileName}";
                        imagePaths.Add(relativePath);

                        if (mainImage == null)
                        {
                            mainImage = relativePath;
                        }
                    }
                }
            }

            mainImage ??= imagePaths.FirstOrDefault();

            model.MainImage = mainImage;

            await _itemService.CreateItemAsync(model, user.Id);
            return RedirectToAction("UserItems");
        }

        [HttpGet]
        public async Task<IActionResult> UserItems([FromQuery] ItemFilterDto filter)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            filter ??= new ItemFilterDto();
            filter.UserId = user.Id;

            filter.Normalize();

            if (string.IsNullOrEmpty(filter.SortOrder))
            {
                filter.SortOrder = "az";
            }

            var items = await _itemService.GetFilteredItemsAsync(filter);

            ViewData["Name"] = filter.Name;
            ViewData["MinPrice"] = filter.MinPrice;
            ViewData["MaxPrice"] = filter.MaxPrice;
            ViewData["CreatedAfter"] = filter.CreatedAfter?.ToString("yyyy-MM-dd");
            ViewData["CreatedBefore"] = filter.CreatedBefore?.ToString("yyyy-MM-dd");
            ViewData["SortOrder"] = filter.SortOrder;
            ViewData["IncludeSold"] = filter.IncludeSold;

            ViewData["Category"] = filter.Category?.ToString();//****************** 
            ViewData["LocalizedCategories"] = _categoryLocalizationService.GetLocalizedCategories();

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !await _itemService.DeleteItemAsync(id, user.Id))
            {
                return Json(new { success = false, message = "Item could not be deleted." });
            }

            return Json(new { success = true });
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

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditItemDto model)
        {
            if (!ModelState.IsValid || !_itemValidationService.Validate(model, ModelState))
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

            return RedirectToAction("UserItems");
        }
    }
}
