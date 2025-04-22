using Karakatsiya.Data;
using Karakatsiya.Interfaces;
using Karakatsiya.Models.DTOs;
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
                ItemImagePath = item.MainImage
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Sale>> GetFilteredSalesAsync(SaleFilterDto filter)
        {
            filter.Normalize();

            var salesQuery = _context.Sales.AsQueryable();

            if (!string.IsNullOrEmpty(filter.ItemName))
                salesQuery = salesQuery.Where(s => s.Item.Name.Contains(filter.ItemName));

            if (filter.StartDate.HasValue)
                salesQuery = salesQuery.Where(s => s.SaleDate >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                salesQuery = salesQuery.Where(s => s.SaleDate <= filter.EndDate.Value);

            if (filter.MinPrice.HasValue)
                salesQuery = salesQuery.Where(s => s.SalePrice >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                salesQuery = salesQuery.Where(s => s.SalePrice <= filter.MaxPrice.Value);

            if (filter.MinProfit.HasValue)
                salesQuery = salesQuery.Where(s => s.Profit >= filter.MinProfit.Value);

            if (filter.MaxProfit.HasValue)
                salesQuery = salesQuery.Where(s => s.Profit <= filter.MaxProfit.Value);

            salesQuery = filter.SortOrder switch
            {
                "name_asc" => salesQuery.OrderBy(s => s.Item.Name),
                "name_desc" => salesQuery.OrderByDescending(s => s.Item.Name),
                "profit_asc" => salesQuery.OrderBy(s => s.Profit),
                "profit_desc" => salesQuery.OrderByDescending(s => s.Profit),
                _ => salesQuery.OrderByDescending(s => s.SaleDate)
            };

            var sales = await salesQuery.ToListAsync();
            return sales;
        }
    }
}
