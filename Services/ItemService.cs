using Karakatsiya.Data;
using Karakatsiya.Interfaces;
using Karakatsiya.Models.DTOs;
using Karakatsiya.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Karakatsiya.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;

        public ItemService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Item> CreateItemAsync(CreateItemDto model, string userId)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            string? imagePath = null;

            if (model.ImageFile?.Length > 0)
            {
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imagePath = $"/images/{fileName}";
            }

            var utcExpirationDate = model.ExpirationDate.HasValue
                ? DateTime.SpecifyKind(model.ExpirationDate.Value, DateTimeKind.Utc)
                : DateTime.SpecifyKind(new DateTime(2099, 12, 31), DateTimeKind.Utc);

            var item = new Item
            {
                Name = model.Name ?? throw new ArgumentNullException(nameof(model.Name)),
                CreationDate = DateTime.UtcNow,
                ExpirationDate = utcExpirationDate,
                ImagePath = imagePath ?? string.Empty,
                Description = model.Description ?? throw new ArgumentNullException(nameof(model.Description)),
                Category = model.Category,
                Price = model.Price,
                Currency = model.Currency ?? string.Empty,
                UserId = userId ?? throw new ArgumentNullException(nameof(userId)),
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
            if (item == null)
            {
                return null;
            }

            item.Name = model.Name ?? item.Name;
            item.Description = model.Description ?? item.Description;
            item.ExpirationDate = model.ExpirationDate.HasValue
                ? DateTime.SpecifyKind(model.ExpirationDate.Value, DateTimeKind.Utc)
                : (DateTime?)null;
            item.Category = model.Category;
            item.Price = model.Price != 0 ? model.Price : item.Price;
            item.Currency = model.Currency ?? item.Currency;

            if (model.ImageFile?.Length > 0)
            {
                if (!string.IsNullOrEmpty(item.ImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.ImagePath.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                item.ImagePath = $"/images/{fileName}";
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

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Item> GetItemDetailsAsync(int id)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
        }

        public async Task<List<Item>> GetUserItemsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            return await _context.Items.Where(i => i.UserId == userId).ToListAsync();
        }
    }
}