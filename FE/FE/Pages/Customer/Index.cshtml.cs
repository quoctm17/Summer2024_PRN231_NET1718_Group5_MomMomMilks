using FE.Helpers;
using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace FE.Pages.Customer
{
    public class IndexModel : PageModel
    {
        private readonly OrderService _orderService;

        public IndexModel(OrderService orderService)
        {
            _orderService = orderService;
        }

        public List<OrderHistory> OrderHistories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? userId)
        {
            var account = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");

            if (account == null || userId == null || account.Id != userId)
            {
                return RedirectToPage("/Login");
            }

            OrderHistories = await _orderService.GetAllOrderHistory(userId.Value);
            return Page();
        }
    }
}
