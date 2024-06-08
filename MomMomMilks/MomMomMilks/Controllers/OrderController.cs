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

        [HttpPost("simple")]
        public async Task<IActionResult> PostSimpleOrder([FromBody] OrderSimpleDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                CreateAt = orderDto.CreateAt,
                UpdateAt = orderDto.UpdateAt,
                TotalAmount = orderDto.TotalAmount,
                BuyerId = orderDto.BuyerId,
                AddressId = orderDto.AddressId,
                PaymentTypeId = orderDto.PaymentTypeId,
                OrderStatusId = orderDto.OrderStatusId,
                TimeSlotId = orderDto.TimeSlotId,
                OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
                {
                    MilkId = od.MilkId,
                    Quantity = od.Quantity,
                    Discount = od.Discount,
                    Price = od.Price,
                    Total = od.Total
                }).ToList()
            };

            try
            {
                await _orderService.CreateOrderAsync(order, order.OrderDetails.ToList());
                return CreatedAtAction(nameof(Get), new { key = order.Id }, order);
            }
            catch (Exception ex)
            {
                // Log detailed error message and inner exception details
                Console.WriteLine($"Error in PostSimpleOrder: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message, innerDetails = ex.InnerException?.Message });
            }
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
