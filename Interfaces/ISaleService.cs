using Karakatsiya.Models.Entities;

namespace Karakatsiya.Interfaces
{
    public interface ISaleService
    {
        Task<Sale> SellItemAsync(int itemId, decimal salePrice, string userId);
        Task<List<Sale>> GetSalesAsync(string userId, DateTime? startDate, DateTime? endDate);
        Task<bool> DeleteSaleAsync(int saleId);
    }
}
