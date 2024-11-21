// File: Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using RealEstate.Dto.Request;
using RealEstate.Dto.Response;
using RealEstate.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RealEstate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var response = await _userService.AuthenticateAsync(loginDto);
            if (response == null)
                return Unauthorized(new { Message = "Invalid username or password" });

            return Ok(response);
        }
    }
}