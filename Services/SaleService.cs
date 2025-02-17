using Karakatsiya.Data;
using Karakatsiya.Interfaces;
using Karakatsiya.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karakatsiya.Services
{
    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> SellItemAsync(int itemId, decimal salePrice, string userId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == itemId && i.UserId == userId);
            if (item == null)
            {
                return null;
            }

            string imagePath = item.ImagePath ?? "/images/Logo_v1.png";

            var sale = new Sale
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Description = item.Description,
                SaleDate = DateTime.UtcNow,
                SalePrice = salePrice,
                Profit = salePrice - item.Price,
                Currency = item.Currency,
                ItemIsDeleted = item.IsDeleted,
                ItemImagePath = imagePath
            };

            _context.Sales.Add(sale);
            item.IsSold = true;
            await _context.SaveChangesAsync();

            return sale;
        }

        public async Task<List<Sale>> GetSalesAsync(string userId, DateTime? startDate, DateTime? endDate)
        {
            var salesQuery = _context.Sales.Include(s => s.Item).Where(s => s.Item.UserId == userId || s.Item == null);

            if (startDate.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.SaleDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.SaleDate <= endDate.Value);
            }

            return await salesQuery.ToListAsync();
        }

        public async Task<bool> DeleteSaleAsync(int saleId)
        {
            var sale = await _context.Sales.FindAsync(saleId);
            if (sale == null)
            {
                return false;
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
