using AutoMapper;
using Karakatsiya.Data;
using Karakatsiya.Interfaces;
using Karakatsiya.Models.DTOs;
using Karakatsiya.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karakatsiya.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;//******

        public ItemService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Item> CreateItemAsync(CreateItemDto model, string userId)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (model.ImageFiles == null || model.ImageFiles.Count == 0)
                throw new ArgumentException("Необходимо загрузить хотя бы одно изображение.");

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var imagePaths = new List<string>();
            foreach (var file in model.ImageFiles.Take(5))
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = $"/images/{fileName}";
                imagePaths.Add(relativePath);
            }

            DateTime? expirationDate = model.ExpirationDate?.ToUniversalTime()
                ?? DateTime.SpecifyKind(new DateTime(2099, 12, 31), DateTimeKind.Utc);

            string mainImage = model.MainImage;
            if (string.IsNullOrEmpty(mainImage))
            {
                mainImage = imagePaths.FirstOrDefault();
            }

            var item = new Item
            {
                Name = model.Name,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = expirationDate,
                ImagePaths = imagePaths,
                MainImage = mainImage,
                Description = model.Description,
                Category = model.Category,
                Price = model.Price,
                Currency = model.Currency,
                UserId = userId
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<Item> EditItemAsync(int id, EditItemDto model, string userId)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id && i.UserId == userId);
            if (item == null) return null;

            item.Name = model.Name;
            item.Description = model.Description;
            item.ExpirationDate = model.ExpirationDate.HasValue
                ? DateTime.SpecifyKind(model.ExpirationDate.Value, DateTimeKind.Utc)
                : item.ExpirationDate;
            item.Category = model.Category;
            item.Price = model.Price;
            item.Currency = model.Currency;

            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                foreach (var file in model.ImageFiles)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    item.ImagePaths.Add($"/images/{fileName}");
                }
            }

            _context.Items.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<bool> DeleteItemAsync(int id, string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            var item = await _context.Items.FindAsync(id);
            if (item == null || item.UserId != userId)
            {
                return false;
            }

            var entityInDb = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.ItemId == id);
            if (entityInDb == null)
            {
                return false;
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Item> GetItemDetailsAsync(int id)
        {
            return await _context.Items
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.ItemId == id);
        }

        public async Task<List<Item>> GetUserItemsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            return await _context.Items.Where(i => i.UserId == userId).ToListAsync();
        }

        //*************************
        public async Task<List<Item>> GetFilteredItemsAsync(ItemFilterDto filter)
        {
            var query = _context.Items
                .Include(i => i.Sales)
                .Where(i => !i.IsDeleted)
                .AsQueryable();

            if (!filter.IncludeSold)
                query = query.Where(i => !i.IsSold);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(i => i.Name.Contains(filter.Name));

            if (filter.MinPrice.HasValue)
                query = query.Where(i => i.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(i => i.Price <= filter.MaxPrice.Value);

            if (filter.CreatedAfter.HasValue)
                query = query.Where(i => i.CreationDate >= filter.CreatedAfter.Value);

            if (filter.CreatedBefore.HasValue)
                query = query.Where(i => i.CreationDate <= filter.CreatedBefore.Value);

            if (filter.SoldAfter.HasValue)
                query = query.Where(i => i.Sales.Any(s => s.SaleDate >= filter.SoldAfter.Value));

            if (filter.SoldBefore.HasValue)
                query = query.Where(i => i.Sales.Any(s => s.SaleDate <= filter.SoldBefore.Value));

            query = filter.SortBy switch
            {
                "name" => filter.Ascending ? query.OrderBy(i => i.Name) : query.OrderByDescending(i => i.Name),
                "price" => filter.Ascending ? query.OrderBy(i => i.Price) : query.OrderByDescending(i => i.Price),
                "creationDate" => filter.Ascending ? query.OrderBy(i => i.CreationDate) : query.OrderByDescending(i => i.CreationDate),
                "saleDate" => filter.Ascending
                    ? query.OrderBy(i => i.Sales.Min(s => s.SaleDate))
                    : query.OrderByDescending(i => i.Sales.Max(s => s.SaleDate)),
                _ => query.OrderBy(i => i.ItemId)
            };

            var items = await query.ToListAsync();

            return _mapper.Map<List<Item>>(items);
        }
    }
}