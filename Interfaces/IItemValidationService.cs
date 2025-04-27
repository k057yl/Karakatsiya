using Karakatsiya.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Karakatsiya.Interfaces
{
    public interface IItemValidationService
    {
        bool Validate(CreateItemDto model, ModelStateDictionary modelState);
        bool Validate(EditItemDto model, ModelStateDictionary modelState);
    }
}
