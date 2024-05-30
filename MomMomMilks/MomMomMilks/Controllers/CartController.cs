using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class CartController : ODataController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            int userId = 1; // Temporary userId for testing
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart.CartItems);
        }

        [HttpPost]
        [Route("AddItem")]
        public async Task<IActionResult> AddItem(int milkId, int quantity)
        {
            int userId = 1; // Temporary userId for testing
            await _cartService.AddCartItemAsync(userId, milkId, quantity);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateItem")]
        public async Task<IActionResult> UpdateItem(int milkId, int quantity)
        {
            int userId = 1; // Temporary userId for testing
            await _cartService.UpdateCartItemAsync(userId, milkId, quantity);
            return NoContent();
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> Delete(int cartItemId)
        {
            int userId = 1; // Temporary userId for testing
            await _cartService.RemoveCartItemAsync(userId, cartItemId);
            return NoContent();
        }

        [HttpPost("ClearCart")]
        public async Task<IActionResult> ClearCart()
        {
            int userId = 1; // Temporary userId for testing
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }
    }
}
