using FE.Models;
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
        public Dictionary<int, MilkCheckedModel> MilkChecked { get; set; }

        [BindProperty]
        public ShipperOrderDetail Order{ get; set; }
        [BindProperty]
        public int OrderId { get; set; }
        public async Task OnGet(int orderId)
        {
            Order = await _orderService.GetShipperOrder(orderId);
            OrderId = orderId;
        }

        public async Task<IActionResult> OnPostConfirmShipped()
        {
            var orderId = 26;
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
            var orderId = 26;


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
        public async Task<IActionResult> OnPostRefund()
        {

            var checkedItems = MilkChecked.Where(x => x.Value.IsChecked).ToList();
            List<Refund> refunds = new List<Refund>();
            foreach (var item in checkedItems)
            {
                int orderDetailId = item.Key;
                string reason = item.Value.Reason;

                refunds.Add(new Refund
                {
                    OrderDetailId = orderDetailId,
                    Note = reason
                });

                // Do something with the milkId and reason
                Console.WriteLine($"Order detail ID: {orderDetailId}, Reason: {reason}");
            }

            if(refunds.Count > 0)
            {
                await _orderService.Refund(refunds);
            }

            // Return a result (e.g., redirect to another page, return a view, etc.)
            return RedirectToPage("/shipper/index");
        }
    }
    public class MilkCheckedModel
    {
        public bool IsChecked { get; set; }
        public string Reason { get; set; }
    }
}
