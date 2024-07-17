using FE.Models.Shipper;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Shipper
{
    public class ConfirmOrderModel : PageModel
    {
        private readonly OrderService _orderService;

        public ConfirmOrderModel(OrderService orderService)
        {
            _orderService = orderService;
        }
        [BindProperty]
        public ShipperOrderDetail Order { get; set; }
        public async Task OnGetAsync(int orderId)
        {
            await _orderService.ConfirmShippedShipperOrder(orderId);
            Order = await _orderService.GetShipperOrder(orderId);
        }
    }
}
