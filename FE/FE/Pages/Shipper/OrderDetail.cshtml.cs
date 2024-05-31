using FE.Models.Shipper;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Shipper
{
    public class OrderDetailModel : PageModel
    {
        private readonly OrderService _orderService;

        public OrderDetailModel(OrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public ShipperOrderDetail Order{ get; set; }
        public async Task OnGet(int orderId)
        {
            Order = await _orderService.GetShipperOrder(orderId);
        }

        public async Task<IActionResult> OnPostConfirmShipped()
        {
            var orderId = Order.Id;
            var result = await _orderService.ConfirmShippedShipperOrder(orderId);
            if(!result)
            {
                ModelState.AddModelError("Lỗi", "Lỗi khi xác nhận");
            } 
            else
            {
                Order.OrderStatusName = "Đã Giao";
            }
            Order = await _orderService.GetShipperOrder(orderId);
            return Page();
        }
        public async Task<IActionResult> OnPostConfirmCancelled()
        {
            var orderId = Order.Id;
            var result = await _orderService.ConfirmCancelledShipperOrder(orderId);
            if (!result)
            {
                ModelState.AddModelError("Lỗi", "Lỗi khi xác nhận");
            }
            else
            {
                Order.OrderStatusName = "Đã Hủy";
            }
            Order = await _orderService.GetShipperOrder(orderId);
            return Page();
        }
    }
}
