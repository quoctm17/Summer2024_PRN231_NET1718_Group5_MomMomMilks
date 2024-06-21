using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomMomMilks.Extensions;
using MomMomMilks.Types;
using Net.payOS.Types;
using Net.payOS;
using BusinessObject.Entities;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly ILogger<PaymentController> _logger;
        private readonly ICartService _cartService;

        public PaymentController(
            PayOS payOS,
            ILogger<PaymentController> logger,
            ICartService cartService
            )
        {
            _payOS = payOS;
            _logger = logger;
            _cartService = cartService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentLink(CreatePaymentLinkRequest body)
        {
            //Note: 
            //1. Description của CreatePaymentLinkRequest ko được quá 20 ký tự
            //2. Tui có add PaymentOrderCode cho cart sau khi tạo link thanh toán thành công.
            //3. ReturnUrl là url khi thanh toán thực hiện thành công thì PayOS sẽ redirect 
            //4. CancelUrl là url khi thanh toán thất bại thì PayOS sẽ redirect 
            //5. Ông dùng cái ReturnUrl để xác nhận thanh toán luôn
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                //Lấy cart trong database ra
                var cart = await _cartService.GetCartByUserIdAsync(User.GetUserId());
                List<ItemData> items = new List<ItemData>();
                var amount = 0;
                foreach (var cartItem in cart.CartItems)
                {
                    var cartItemPrice = cartItem.Milk.Price * cartItem.Quantity;
                    ItemData item = new ItemData(cartItem.Milk.Name, cartItem.Quantity, int.Parse(cartItemPrice.ToString()));
                    items.Add(item);
                    amount++;
                }
                PaymentData paymentData = new PaymentData(orderCode, amount, body.description, items, body.cancelUrl, body.returnUrl);
                //var result = await _cartService.AddPaymentOrderCode(cart.Id, orderCode);
                //if(!result)
                //{
                //    throw new Exception("Error while adding order code to cart");
                //}
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
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                var status = paymentLinkInformation.status == "PAID";
                if (status)
                {
                    //**Note**
                    //1. Ông tham khảo cách tui làm bên dưới nhé(cái ở dưới là tui làm cho app EXE, nó sẽ lấy
                    //payment từ database ra dựa vào order code r xác nhận thanh toán)
                    //2. Ông lấy cái cart trong database ra (lấy bằng PaymentOrderCode == paymentLinkInformation.orderCode)
                    //3. Ông viết hàm chuyển từ Cart sang Order là xog

                    //***************Start Example***************
                    //var intent = await _mealPlanRepository.GetPaymentIntent(paymentLinkInformation.orderCode);
                    //if (intent != null)
                    //{
                    //    await _mealPlanRepository.RegistMealPlan(intent.AppUserId, intent.MealPlanId);
                    //    var transaction = new TransactionDTO
                    //    {
                    //        SenderId = intent.AppUserId,
                    //        ReportName = $"User {intent.AppUserId} đã thực hiện giao dịch mua meal plan {intent.MealPlanId}",
                    //        CreateDate = DateTime.UtcNow,
                    //        TotalPrice = paymentLinkInformation.amountPaid
                    //    };
                    //    await _userRepository.AddTransaction(transaction);
                    //}
                    //***************End Example***************
                }
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
    }
}
