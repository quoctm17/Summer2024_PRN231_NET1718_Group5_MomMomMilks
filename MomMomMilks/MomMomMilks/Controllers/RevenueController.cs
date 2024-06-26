using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class RevenueController : ODataController
    {
        private readonly IOrderService _orderService;
        public RevenueController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetOrdersToCalculateRevenue();
            return Ok(orders);
        }
    }
}
