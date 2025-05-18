using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Models.DTOs;
using Karakatsiya.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Karakatsiya.Controllers
{
    [Authorize]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IItemValidationService _itemValidationService;
        private readonly CategoryLocalizationService _categoryLocalizationService;
        private readonly SharedLocalizationService _localizer;

        public ItemController(
            IItemService itemService,
            UserManager<IdentityUser> userManager,
            IItemValidationService itemValidationService,
            CategoryLocalizationService categoryLocalizationService,
            SharedLocalizationService sharedLocalizationService)
            : base()
        {
            _itemService = itemService;
            _userManager = userManager;
            _itemValidationService = itemValidationService;
            _categoryLocalizationService = categoryLocalizationService;
            _localizer = sharedLocalizationService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateItem()
        {
            var localizedCategories = await _categoryLocalizationService.GetLocalizedCategoriesAsync();

            var model = new CreateItemDto
            {
                Categories = localizedCategories
                    .OrderBy(c => c.Value)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Key.ToString(),
                        Text = c.Value
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemDto model)
        {
            if (model.ImageFiles == null || model.ImageFiles.Count == 0)
            {
                ModelState.AddModelError(nameof(model.ImageFiles), _localizer.WarningMessages["NeedToSelectImage"]);
            }

            if (!ModelState.IsValid || !_itemValidationService.Validate(model, ModelState))
            {
                var localizedCategories = await _categoryLocalizationService.GetLocalizedCategoriesAsync();

                model.Categories = localizedCategories
                    .OrderBy(c => c.Value)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Key.ToString(),
                        Text = c.Value
                    }).ToList();

                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View(model);
            }

            await _itemService.CreateItemAsync(model, user.Id);

            return RedirectToAction(ProjectConstants.PAGE_CATEGORY);
        }

        [HttpGet]
        public async Task<IActionResult> ByCategory ([FromQuery] ItemFilterDto filter)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(ProjectConstants.CONTROLLER_ACCOUNT, ProjectConstants.PAGE_LOGIN);
            }

            filter ??= new ItemFilterDto();
            filter.UserId = user.Id;

            filter.Normalize();

            if (string.IsNullOrEmpty(filter.SortOrder))
            {
                filter.SortOrder = "az";
            }

            var items = await _itemService.GetFilteredItemsAsync(filter);

            var categories = await _categoryLocalizationService.GetLocalizedCategoriesAsync();

            ViewData["Name"] = filter.Name;
            ViewData["MinPrice"] = filter.MinPrice;
            ViewData["MaxPrice"] = filter.MaxPrice;
            ViewData["CreatedAfter"] = filter.CreatedAfter?.ToString("yyyy-MM-dd");
            ViewData["CreatedBefore"] = filter.CreatedBefore?.ToString("yyyy-MM-dd");
            ViewData["SortOrder"] = filter.SortOrder;
            ViewData["IncludeSold"] = filter.IncludeSold;
            ViewData["Category"] = filter.CategoryId;
            ViewData["Categories"] = categories.Select(c => new SelectListItem
            {
                Value = c.Key.ToString(),
                Text = c.Value
            }).ToList();

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !await _itemService.DeleteItemAsync(id, user.Id))
            {
                return Json(new { success = false, message = _localizer.WarningMessages["ItemCouldNotBeDeleted"] });
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
                return RedirectToAction(ProjectConstants.CONTROLLER_ACCOUNT, ProjectConstants.PAGE_LOGIN);
            }

            var item = await _itemService.EditItemAsync(id, model, user.Id);
            if (item == null)
            {
                return NotFound();
            }

            return RedirectToAction(ProjectConstants.PAGE_CATEGORY);
        }
        
    }
}