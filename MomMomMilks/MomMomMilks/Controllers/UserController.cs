using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace MomMomMilks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("shippers")]
        public async Task<IActionResult> GetAllShippers()
        {
            return Ok(await _userRepository.GetAllShippers());
        }
    }
}
