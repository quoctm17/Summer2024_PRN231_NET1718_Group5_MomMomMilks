using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using MomMomMilks.Extensions;
using Repository;
using Repository.Interface;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class OrderController : ODataController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequestDto orderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _orderService.CreateOrderAsync(orderRequest.Order, orderRequest.OrderDetails);
            return Created(orderRequest.Order);
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var order = await _orderService.GetOrderAsync(key);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [EnableQuery]
        [HttpGet("OrderHistory({userId})")]
        public async Task<IActionResult> GetOrderHistoryByUserId([FromODataUri] int userId)
        {
            var orderHistory = await _orderService.GetAllOrderHistory(userId);
            if (orderHistory == null)
            {
                return NotFound();
            }
            return Ok(orderHistory);
        }
        [EnableQuery]
        [HttpGet("OrderDetail({orderId})")]
        public async Task<IActionResult> GetOrderDetailHistory([FromODataUri] int orderId)
        {
            var orderDetail = await _orderService.GetDetailHistory(orderId);
            if(orderDetail == null)
            {
                return NotFound();
            }
            return Ok(orderDetail);
        }

        [EnableQuery]
        [HttpGet("ShipperOrders")]
        [Authorize]
        public async Task<IActionResult> GetShipperOrders()
        {
            var userId = User.GetUserId();
            var result = await _orderService.GetShipperOrders(userId);
            return Ok(result);
        }
        [EnableQuery]
        [HttpGet("ShipperOrders({orderId})")]
        [Authorize]
        public async Task<IActionResult> GetShipperOrder([FromODataUri] int orderId)
        {
            var userId = User.GetUserId();
            var result = await _orderService.GetShipperOrderDetail(userId, orderId);
            return Ok(result);
        }
        [HttpPut("ShipperOrders/confirm({orderId})")]
        [Authorize]
        public async Task<IActionResult> ConfirmShippedShipperOrder([FromODataUri] int orderId)
        {
            var userId = User.GetUserId();
            var result = await _orderService.ConfirmShipped(userId, orderId);
            return Ok(result);
        }
        [HttpPut("ShipperOrders/cancel({orderId})")]
        [Authorize]
        public async Task<IActionResult> ConfirmCancelledShipperOrder([FromODataUri] int orderId)
        {
            var userId = User.GetUserId();
            var result = await _orderService.ConfirmCancelled(userId, orderId);
            return Ok(result);
        }
    }
}
