using FE.Helpers;
using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

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
        public async Task OnGetAsync()
        {
            var account = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
            OrderHistories = await _orderService.GetAllOrderHistory(account.Id);
        }
    }
}
