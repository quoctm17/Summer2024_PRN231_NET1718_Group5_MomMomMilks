using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomMomMilks.Extensions;
using MomMomMilks.Types;
using Net.payOS.Types;
using Net.payOS;
using BusinessObject.Entities;
using Service.Interfaces;
using Service.Services;
using System.Runtime.Intrinsics.X86;
using DataTransfer;

namespace MomMomMilks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly ILogger<PaymentController> _logger;
        private readonly IOrderService _orderService;
        private readonly ITransactionService _transactionService;

        public PaymentController(
            PayOS payOS,
            ILogger<PaymentController> logger,
            IOrderService orderService,
            ITransactionService transactionService
            )
        {
            _payOS = payOS;
            _logger = logger;
            _orderService = orderService;
            _transactionService = transactionService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentLink(CreatePaymentLinkRequest body)
        {
            try
            {

                var order = await _orderService.GetOrderAsync(body.orderId);
                if (order == null)
                {
                    throw new Exception("Order not found");
                }
                long orderCode = GenerateUniqueOrderCode(order.Id);

                if (order.TotalAmount <= 0)
                {
                    throw new Exception("Order total amount must be greater than 0");
                }

                List<ItemData> items = new List<ItemData>();
                foreach (var orderDetail in order.OrderDetails)
                {
                    if (orderDetail.Price <= 0)
                    {
                        throw new Exception("Order detail price must be greater than 0");
                    }

                    var itemPrice = (int)(orderDetail.Price * 1000); // Convert prices to int units
                    ItemData item = new ItemData(orderDetail.Milk.Name, orderDetail.Quantity, itemPrice);
                    items.Add(item);
                }

                var totalAmountInCents = (int)(order.TotalAmount / 100);
                if (totalAmountInCents <= 0)
                {
                    throw new Exception("Total amount in cents must be greater than 0");
                }

                PaymentData paymentData = new PaymentData(orderCode, totalAmountInCents, body.description, items, body.cancelUrl, body.returnUrl);

                // Log payment data for debugging
                _logger.LogInformation("PaymentData: {0}", paymentData);

                var result = await _orderService.AddPaymentOrderCode(order.Id, orderCode);
                if (!result)
                {
                    throw new Exception("Error while adding order code to order");
                }

                // Update the transaction with the PaymentOrderCode
                var transaction = await _transactionService.GetTransactionByOrderIdAsync(order.Id);
                if (transaction != null)
                {
                    transaction.PaymentOrderCode = orderCode.ToString();
                    await _transactionService.UpdateTransactionAsync(transaction);
                }

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return Ok(new Response(0, "success", createPayment));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }


        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrder([FromRoute] long orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                var status = paymentLinkInformation.status == "PAID";
                if (status)
                {
                    var transaction = await _transactionService.GetTransactionByOrderPaymentCodeAsync(paymentLinkInformation.orderCode);
                    if (transaction != null)
                    {
                        var order = await _orderService.GetOrderByPaymentOrderCode(paymentLinkInformation.orderCode);
                        if (order != null)
                        {
                            // Confirm payment and update order status
                            order.OrderStatusId = 2; // Assigning
                            await _orderService.UpdateOrder(order);

                            // Update transaction status
                            transaction.Status = "Paid";
                            await _transactionService.UpdateTransactionAsync(transaction);
                        }
                    }
                }
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }

        [HttpPost("cancel")]
        [Authorize]
        public async Task<IActionResult> CancelPaymentLink(CancelPaymentLinkRequest body)
        {
            try
            {
                var order = await _orderService.GetOrderByPaymentOrderCode(body.orderCode);
                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                var paymentLinkInformation = await _payOS.cancelPaymentLink(body.orderCode, body.cancellationReason);

                // Update order status based on current status
                if (order.OrderStatusId == 1) // Assuming 1 is 'Paying'
                {
                    order.OrderStatusId = 5; // Assuming 5 represents 'Cancelled'
                }
                else if (order.OrderStatusId == 2) // Assuming 2 is 'Assigning'
                {
                    order.OrderStatusId = 6; // Assuming 6 represents 'Return Refund'
                }
                await _orderService.UpdateOrder(order);

                // Create a new transaction for the refund
                var refundTransaction = new BusinessObject.Entities.Transaction
                {
                    OrderId = order.Id,
                    Status = "Refunding",
                    CreatedAt = DateTime.Now,
                    GrossAmount = order.TotalAmount,
                    PaymentOrderCode = null // Refund doesn't need a PaymentOrderCode
                };
                await _transactionService.AddTransactionAsync(refundTransaction);

                return Ok(new Response(0, "success", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }


        [HttpPost("FetchOrderByDateAndUserId")]
        [Authorize]
        public async Task<IActionResult> FetchOrderByDateAndUserId([FromBody] FetchOrderRequestDTO request)
        {
            try
            {
                var order = await _orderService.GetOrderByBuyerIdAndCreateAt(request.UserId, request.CreateAt);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message, innerDetails = ex.InnerException?.Message });
            }
        }

        // Unique OrderCode constructor
        private long GenerateUniqueOrderCode(int orderId)
        {
            // Combine OrderId and current time in ticks to ensure uniqueness
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return orderId * 100000 + (timestamp % 100000);
        }

    }
}
