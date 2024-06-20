using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Authorization;
using DataTransfer;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class CartController : ODataController
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                //var userId = GetUserIdFromToken();
                //if (userId == null)
                //{
                //    return Unauthorized("User not logged in");
                //}

                var cart = await _cartService.GetCartByUserIdAsync(1);
                if (cart == null)
                {
                    return NotFound();
                }

                var cartItems = cart.CartItems.Select(item => new
                {
                    MilkId = item.MilkId,
                    Quantity = item.Quantity,
                    Milk = new
                    {
                        item.Milk.Id,
                        item.Milk.Name,
                        item.Milk.Price,
                        item.Milk.ImageUrl
                    }
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching cart.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        [Route("AddItem")]
        public async Task<IActionResult> AddItem(int milkId, int quantity)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null)
                {
                    return Unauthorized("User not logged in");
                }

                await _cartService.AddCartItemAsync(userId.Value, milkId, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding item to cart.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdateItem")]
        public async Task<IActionResult> UpdateItem(int milkId, int quantity)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null)
                {
                    return Unauthorized("User not logged in");
                }

                await _cartService.UpdateCartItemAsync(userId.Value, milkId, quantity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating cart item.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> Delete(int cartItemId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null)
                {
                    return Unauthorized("User not logged in");
                }

                await _cartService.RemoveCartItemAsync(userId.Value, cartItemId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting cart item.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("ClearCart")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null)
                {
                    return Unauthorized("User not logged in");
                }

                await _cartService.ClearCartAsync(userId.Value);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while clearing cart.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("SaveCart")]
        [Authorize]
        public async Task<IActionResult> SaveCart([FromBody] List<CartItemDTO> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
            {
                _logger.LogWarning("SaveCart: Cart items cannot be null or empty");
                return BadRequest("Cart items cannot be null or empty");
            }

            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null)
                {
                    _logger.LogWarning("SaveCart: User not logged in");
                    return Unauthorized("User not logged in");
                }

                var cartItemEntities = cartItems.Select(item => new CartItem
                {
                    MilkId = item.MilkId,
                    Quantity = item.Quantity
                }).ToList();

                _logger.LogInformation($"SaveCart: Saving cart for user {userId} with {cartItemEntities.Count} items");

                await _cartService.SaveCartAsync(userId.Value, cartItemEntities);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving cart.");
                return StatusCode(500, "Internal server error");
            }
        }


        private int? GetUserIdFromToken()
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                _logger.LogInformation($"Authorization header: {authHeader}");
                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    _logger.LogInformation($"Token: {token}");
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    if (jwtToken != null)
                    {
                        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId);
                        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                        {
                            _logger.LogInformation($"User ID from token: {userId}");
                            return userId;
                        }
                        else
                        {
                            _logger.LogWarning("User ID claim not found or invalid");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("JWT token is null");
                    }
                }
                else
                {
                    _logger.LogWarning("Authorization header is null or doesn't start with 'Bearer '");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while extracting UserId from token.");
            }
            return null;
        }
    }
}
