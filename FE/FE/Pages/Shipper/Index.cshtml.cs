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
    }
}
