using Karakatsiya.Interfaces;
using Karakatsiya.Models.DTOs;
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

        public async Task<IActionResult> Sales(SaleFilterDto filter)
        {
            filter.Normalize();

            var sales = await _saleService.GetFilteredSalesAsync(filter);

            ViewData["StartDate"] = filter.StartDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = filter.EndDate?.ToString("yyyy-MM-dd");
            ViewData["MinPrice"] = filter.MinPrice;
            ViewData["MaxPrice"] = filter.MaxPrice;
            ViewData["MinProfit"] = filter.MinProfit;
            ViewData["MaxProfit"] = filter.MaxProfit;
            ViewData["SortOrder"] = filter.SortOrder;

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
