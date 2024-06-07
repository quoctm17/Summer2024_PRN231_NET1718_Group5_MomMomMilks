using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class AddressController : ODataController
    {
        private readonly IAddressService _addressService;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressService addressService, ILogger<AddressController> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesAsync();
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching addresses.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressById(int addressId)
        {
            try
            {
                var address = await _addressService.GetAddressByIdAsync(addressId);
                if (address == null)
                {
                    return NotFound();
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching address by ID.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            try
            {
                await _addressService.AddAddressAsync(address);
                return Created(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding address.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] Address address)
        {
            try
            {
                if (addressId != address.Id)
                {
                    return BadRequest("Address ID mismatch");
                }

                await _addressService.UpdateAddressAsync(address);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating address.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            try
            {
                await _addressService.DeleteAddressAsync(addressId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting address.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAddressesByUserIdAsync(int userId)
        {
            try
            {
                var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching addresses by user ID.");
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
