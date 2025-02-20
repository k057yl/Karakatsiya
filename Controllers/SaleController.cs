using Karakatsiya.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Karakatsiya.Controllers
{
    [Authorize]
    public class SaleController : BaseController
    {
        private readonly ISaleService _saleService;
        private readonly UserManager<IdentityUser> _userManager;

        public SaleController(ISaleService saleService, UserManager<IdentityUser> userManager)
        {
            _saleService = saleService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Sales(DateTime? startDate, DateTime? endDate)
        {
            var user = await _userManager.GetUserAsync(User);
            var sales = await _saleService.GetSalesAsync(user.Id, startDate, endDate);
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
            return View(sales);
        }

        [HttpPost]
        public async Task<IActionResult> SellItem(int itemId, decimal salePrice, decimal profit)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var sale = await _saleService.SellItemAsync(itemId, salePrice, user.Id);
            if (sale == null)
            {
                return NotFound();
            }

            return RedirectToAction("Sales");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSale(int saleId)
        {
            var success = await _saleService.DeleteSaleAsync(saleId);
            if (!success)
            {
                return Json(new { success = false, message = "Sale not found." });
            }

            return Json(new { success = true });
        }
    }
}
