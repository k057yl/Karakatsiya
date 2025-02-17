using Karakatsiya.Models.DTOs;
using Karakatsiya.Models.Entities;

namespace Karakatsiya.Interfaces
{
    public interface IItemService
    {
        Task<Item> CreateItemAsync(CreateItemDto model, string userId);
        Task<Item> EditItemAsync(int id, EditItemDto model, string userId);
        Task<bool> DeleteItemAsync(int id, string userId);
        Task<Item> GetItemDetailsAsync(int id);
        Task<List<Item>> GetUserItemsAsync(string userId);
    }
}
