using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class UserController : ODataController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                //var userIdFromToken = GetUserIdFromToken();
                //if (userIdFromToken == null)
                //{
                //    return Unauthorized("User not logged in");
                //}

                var user = await _userService.GetUserById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user by ID.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == null)
                {
                    return Unauthorized("User not logged in");
                }

                var user = await _userService.GetUserById(userId.Value);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching current user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AppUser user)
        {
            try
            {
                var userIdFromToken = GetUserIdFromToken();
                if (userIdFromToken == null)
                {
                    return Unauthorized("User not logged in");
                }

                await _userService.AddUser(user);
                return Created(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserDetailDTO user)
        {
            try
            {
                var userIdFromToken = GetUserIdFromToken();
                if (userIdFromToken == null)
                {
                    return Unauthorized("User not logged in");
                }

                if (userId != user.Id)
                {
                    return BadRequest("User ID mismatch");
                }

                var userDetail = await _userService.GetUserById(userId);
                if (userDetail != null)
                {
                    userDetail.UserName = user.Name;
                    userDetail.Email = user.Email;
                    userDetail.PhoneNumber = user.PhoneNumber;
                    userDetail.NormalizedEmail = user.Email.ToUpper();
                    userDetail.NormalizedUserName = user.Name.ToUpper();
                    await _userService.UpdateUser(userDetail);
                    return Ok("Updated Successfully");
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                await _userService.CancelOrder(orderId);
                return Ok("Cancel order successfully");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("refund/{orderId}")]
        public async Task<IActionResult> Refund(int orderId)
        {
            try
            {
                await _userService.RefundOrder(orderId);
                return Ok("Refund order successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var userIdFromToken = GetUserIdFromToken();
                if (userIdFromToken == null)
                {
                    return Unauthorized("User not logged in");
                }

                await _userService.DeleteUser(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("shippers")]
        public async Task<IActionResult> GetAllShippers()
        {
            try
            {
                var userIdFromToken = GetUserIdFromToken();
                if (userIdFromToken == null)
                {
                    return Unauthorized("User not logged in");
                }

                return Ok(await _userService.GetAllShippers());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching shippers.");
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
