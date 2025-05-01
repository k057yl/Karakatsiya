using Karakatsiya.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karakatsiya.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ShowCategories()
        {
            var categories = await _context.Categories.OrderBy(c => c.SortOrder).ToListAsync();
            return View(categories);
        }
    }
}
