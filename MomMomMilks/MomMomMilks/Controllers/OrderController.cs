using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using MomMomMilks.Exceptions;
using MomMomMilks.Extensions;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class OrderController : ODataController
    {
        private readonly IOrderService _orderService;
        private readonly ICouponService _couponService;
        public OrderController(IOrderService orderService, ICouponService couponService)
        {
            _orderService = orderService;
            _couponService = couponService;
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

            var order = new Order
            {
                CreateAt = orderDto.CreateAt,
                UpdateAt = orderDto.UpdateAt,
                OrderDate = orderDto.OrderDate,
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
                    Total = od.Total,
                    Status = od.Status
                }).ToList()
            };

            try
            {
                Console.WriteLine("Start PostSimpleOrder");
                await _orderService.CreateOrderAsync(order, order.OrderDetails.ToList());
                await _couponService.AddOrderCouponAsync(orderDto.Code, order.Id);
                Console.WriteLine("Order created successfully");
                return CreatedAtAction(nameof(Get), new { key = order.Id }, order);
            }
            catch (OutOfStockException ex)
            {
                Console.WriteLine($"OutOfStockException: {ex.Message}");
                return StatusCode(409, new { message = ex.Message });
            }
            catch (Exception ex)
            {
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
        [HttpGet("OrderStatus")]
        public async Task<IActionResult> GetStatus()
        {
            var orderStatus = await _orderService.GetAllStatus();
            if (orderStatus == null)
            {
                return NotFound();
            }
            return Ok(orderStatus);
        }

        [HttpGet("IsCompleted")]
        public async Task<IActionResult> IsCompletedOrder([FromQuery] int orderId)
        {
            var orderStatus = await _orderService.IsCompletedOrder(orderId);
            if (orderStatus == null)
            {
                return NotFound();
            }
            return Ok(orderStatus);
        }

        [EnableQuery]
        [HttpGet("OrderHistory({userId})")]
        public async Task<IActionResult> GetOrderHistoryByUserId([FromRoute] int userId)
        {
            try
            {
                var orderHistory = await _orderService.GetAllOrderHistory(userId);
                if (orderHistory == null || !orderHistory.Any())
                {
                    return NotFound();
                }

                var orderHistoryDTOs = orderHistory.Select(order => new OrderHistoryDTO
                {
                    Id = order.Id,
                    CreateAt = order.CreateAt,
                    UpdateAt = order.UpdateAt,
                    TotalAmount = order.TotalAmount,
                    BuyerId = order.BuyerId,
                    AddressId = order.AddressId,
                    PaymentType = order.PaymentType?.Name,
                    TransactionId = order.TransactionId,
                    Shipper = order.Shipper?.UserName,
                    OrderStatusId = order.OrderStatusId,
                    PaymentOrderCode = order.PaymentOrderCode
                }).ToList();

                return Ok(orderHistoryDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [EnableQuery]
        [HttpGet("OrderDetail({orderId})")]
        public async Task<IActionResult> GetOrderDetailHistory([FromRoute] int orderId)
        {
            var orderDetail = await _orderService.GetDetailHistory(orderId);
            if (orderDetail == null)
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
        [HttpGet("ManagerOrders")]
        public async Task<IActionResult> GetUnassignedOrders()
        {
            var result = await _orderService.GetUnassignedOrders();
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
        [HttpPut("ManagerOrders/Assign/{shipperId}/{orderId}")]
        public async Task<IActionResult> ManagerAssignOrder([FromODataUri] int orderId, [FromODataUri] int shipperId)
        {
            var result = await _orderService.ManagerAssignOrder(orderId, shipperId);
            return Ok(result);
        }
        [HttpPut("Refund")]
        [Authorize]
        public async Task<IActionResult> ManagerAssignOrder([FromBody] List<RefundDTO> refundDTOs)
        {
            var result = await _orderService.RefundOrder(refundDTOs);
            return Ok(result);
        }
        [EnableQuery]
        [HttpGet("topProduct")]
        public async Task<IActionResult> GetTopProducts([FromQuery] int topN = 10)
        {
            var topProducts = await _orderService.GetTopProducts(topN);
            var topProductsDto = topProducts.Select(tp => new TopProductDto
            {
                MilkId = tp.Milk.Id,
                MilkName = tp.Milk.Name,
                TotalQuantitySold = tp.TotalQuantitySold
            }).ToList();

            return Ok(topProductsDto);
        }

        // Test method to manually trigger auto-assign for testing
        [HttpPost("AutoAssignTest")]
        //[Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> AutoAssignTest([FromQuery] DateTime orderDate, [FromQuery] string timeSlot)
        {
            try
            {
                await _orderService.AutoAssignOrdersToShippers(orderDate, timeSlot);
                return Ok(new { message = "Auto assign completed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("cancel-order")]
        [Authorize]
        public async Task<IActionResult> CancelOrder([FromQuery] int orderId)
        {
            await _orderService.CancelOrder(orderId);
            return Ok("Cancel Order Successfully");
        }

        [HttpPut("confirm-refund")]
        [Authorize/*(Policy = "RequireManagerRole")*/]
        public async Task<IActionResult> ConfirmRefund([FromQuery] int orderId)

        {
            var result = await _orderService.ConfirmRefund(orderId);
            return Ok(result);

        }
    }
}