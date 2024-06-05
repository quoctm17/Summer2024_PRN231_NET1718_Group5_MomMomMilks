using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, 
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Email.Equals(loginDto.Email));
            if (user == null) return Unauthorized("Invalid Email");
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return Unauthorized("Invalid username or password");
            var role = await _userManager.GetRolesAsync(user);
            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Status = user.Status,
                Role = role.FirstOrDefault(),
                Token = await _tokenService.GenerateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if(registerDto.Password != registerDto.ConfirmedPassword)
            {
                return BadRequest("Password does not match");
            }
            if (!(await UserExist(registerDto.Email)))
            {
                var user = _mapper.Map<AppUser>(registerDto);
                user.Status = 1;
                user.Point = 0;
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);

                var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
                if (!roleResult.Succeeded) return BadRequest(result.Errors);

                return new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Status = user.Status,
                    Point = user.Point,
                    Role = "Customer",
                    Token = await _tokenService.GenerateToken(user),
                };
            }
            else
            {
                return BadRequest("Email is taken");
            }


        }
        private async Task<bool> UserExist(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.Equals(username.ToLower()));
        }
    }
}
