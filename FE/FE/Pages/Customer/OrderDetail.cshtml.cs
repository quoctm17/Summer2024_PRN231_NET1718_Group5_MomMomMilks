using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Customer
{
    public class OrderDetailModel : PageModel
    {
        private readonly OrderService _orderService;
        public OrderDetailModel(OrderService orderService)
        {
            _orderService = orderService;
        }
        
        public List<OrderDetailHistory> OrderDetailHistory { get; set; }
        [BindProperty]
        public int OrderId { get; set; } = default(int);
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if(id == null)
            {
                return NotFound();
            }

            OrderId = id;

            OrderDetailHistory = await _orderService.GetAllOrderDetailHistory(id);
            if(OrderDetailHistory == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
