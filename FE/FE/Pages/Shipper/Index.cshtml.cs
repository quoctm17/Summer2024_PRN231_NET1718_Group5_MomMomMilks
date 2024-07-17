using FE.Helpers;
using FE.Models;
using FE.Models.Shipper;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Shipper
{
    public class IndexModel : PageModel
    {
        private readonly OrderService _orderService;

        public IndexModel(OrderService orderService)
        {
            _orderService = orderService;
        }

        public List<ShipperOrder> ShipperOrders { get; set; }
        [BindProperty]
        public int OrderId { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var user = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
            if(user != null && user.Role == "Shipper")
            {
                var result = await _orderService.GetShipperOrders();
                ShipperOrders = result;
                return Page();
            }
            return RedirectToPage("/login");
            
        }
        public async Task<IActionResult> OnPostConfirmShipped()
        {
            var orderId = OrderId;
            var result = await _orderService.ConfirmShippedShipperOrder(orderId);
            if (!result)
            {
                ModelState.AddModelError("Lỗi", "Lỗi khi xác nhận");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostConfirmCancelled()
        {
            var orderId = OrderId;


            var result = await _orderService.ConfirmCancelledShipperOrder(orderId);
            if (!result)
            {
                ModelState.AddModelError("Lỗi", "Lỗi khi xác nhận");
            }
            return Page();
        }
    }
}
