using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Models.DTOs;
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
        private readonly HtmlValidator _htmlValidator;
        private readonly SharedLocalizationService _localization;

        public ItemController(
            IItemService itemService,
            UserManager<IdentityUser> userManager,
            HtmlValidator htmlValidator,
            SharedLocalizationService localization)
            : base()
        {
            _itemService = itemService;
            _htmlValidator = htmlValidator;
            _userManager = userManager;
            _localization = localization;
        }

        [HttpGet]
        public IActionResult CreateItem()
        {
            return View(new CreateItemDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemDto model)
        {
            if (!ModelState.IsValid || !ValidateHtmlFields(model))
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

            return RedirectToAction("UserItems");
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
            if (!ModelState.IsValid || !ValidateHtmlFields(model))
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

        private bool ValidateHtmlFields(CreateItemDto model)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), @_localization.WarningMessages["NameCannotBeEmpty"]);
                isValid = false;
            }
            else
            {
                var nameValidationResult = _htmlValidator.ValidateHtml(model.Name);
                if (!nameValidationResult.IsValid)
                {
                    ModelState.AddModelError(nameof(model.Name), nameValidationResult.ErrorMessage);
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError(nameof(model.Description), @_localization.WarningMessages["DescriptionCannotBeEmpty"]);
                isValid = false;
            }
            else
            {
                var descriptionValidationResult = _htmlValidator.ValidateHtml(model.Description);
                if (!descriptionValidationResult.IsValid)
                {
                    ModelState.AddModelError(nameof(model.Description), descriptionValidationResult.ErrorMessage);
                    isValid = false;
                }
            }

            if (model.Price <= 0)
            {
                ModelState.AddModelError(nameof(model.Price), @_localization.WarningMessages["PriceCannotBeZero"]);
                isValid = false;
            }
            else
            {
                var priceValidationResult = _htmlValidator.ValidateHtml(model.Price.ToString());
                if (!priceValidationResult.IsValid)
                {
                    ModelState.AddModelError(nameof(model.Price), priceValidationResult.ErrorMessage);
                    isValid = false;
                }
            }

            return isValid;
        }

        private bool ValidateHtmlFields(EditItemDto model)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "Название не может быть пустым.");
                isValid = false;
            }
            else
            {
                var nameValidationResult = _htmlValidator.ValidateHtml(model.Name);
                if (!nameValidationResult.IsValid)
                {
                    ModelState.AddModelError(nameof(model.Name), nameValidationResult.ErrorMessage);
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError(nameof(model.Description), "Описание не может быть пустым.");
                isValid = false;
            }
            else
            {
                var descriptionValidationResult = _htmlValidator.ValidateHtml(model.Description);
                if (!descriptionValidationResult.IsValid)
                {
                    ModelState.AddModelError(nameof(model.Description), descriptionValidationResult.ErrorMessage);
                    isValid = false;
                }
            }

            if (model.Price <= 0)
            {
                ModelState.AddModelError(nameof(model.Price), "Цена должна быть больше нуля.");
                isValid = false;
            }
            else
            {
                var priceValidationResult = _htmlValidator.ValidateHtml(model.Price.ToString());
                if (!priceValidationResult.IsValid)
                {
                    ModelState.AddModelError(nameof(model.Price), priceValidationResult.ErrorMessage);
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
