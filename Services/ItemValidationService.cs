using Karakatsiya.Interfaces;
using Karakatsiya.Localizations;
using Karakatsiya.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Karakatsiya.Services
{
    public class ItemValidationService : IItemValidationService
    {
        private readonly HtmlValidator _htmlValidator;
        private readonly SharedLocalizationService _localization;

        public ItemValidationService(HtmlValidator htmlValidator, SharedLocalizationService localization)
        {
            _htmlValidator = htmlValidator;
            _localization = localization;
        }

        public bool Validate(CreateItemDto model, ModelStateDictionary modelState)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                modelState.AddModelError(nameof(model.Name), _localization.WarningMessages["NameCannotBeEmpty"]);
                isValid = false;
            }
            else
            {
                var nameResult = _htmlValidator.ValidateHtml(model.Name);
                if (!nameResult.IsValid)
                {
                    modelState.AddModelError(nameof(model.Name), nameResult.ErrorMessage);
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                modelState.AddModelError(nameof(model.Description), _localization.WarningMessages["DescriptionCannotBeEmpty"]);
                isValid = false;
            }
            else
            {
                var descResult = _htmlValidator.ValidateHtml(model.Description);
                if (!descResult.IsValid)
                {
                    modelState.AddModelError(nameof(model.Description), descResult.ErrorMessage);
                    isValid = false;
                }
            }

            if (model.Price <= 0)
            {
                modelState.AddModelError(nameof(model.Price), _localization.WarningMessages["PriceCannotBeZero"]);
                isValid = false;
            }

            return isValid;
        }

        public bool Validate(EditItemDto model, ModelStateDictionary modelState)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                modelState.AddModelError(nameof(model.Name), _localization.WarningMessages["NameCannotBeEmpty"]);
                isValid = false;
            }
            else
            {
                var nameResult = _htmlValidator.ValidateHtml(model.Name);
                if (!nameResult.IsValid)
                {
                    modelState.AddModelError(nameof(model.Name), nameResult.ErrorMessage);
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                modelState.AddModelError(nameof(model.Description), _localization.WarningMessages["DescriptionCannotBeEmpty"]);
                isValid = false;
            }
            else
            {
                var descResult = _htmlValidator.ValidateHtml(model.Description);
                if (!descResult.IsValid)
                {
                    modelState.AddModelError(nameof(model.Description), descResult.ErrorMessage);
                    isValid = false;
                }
            }

            if (model.Price <= 0)
            {
                modelState.AddModelError(nameof(model.Price), _localization.WarningMessages["PriceCannotBeZero"]);
                isValid = false;
            }

            return isValid;
        }
    }
}
