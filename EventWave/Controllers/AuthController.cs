using EventWave.DTOs;
using EventWave.Models;
using EventWave.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.Email,
                FullName = dto.FullName
            };

            var success = await _userService.AddUserAsync(user, dto.Password, "Client");
            if (success==null)
                return BadRequest("Registration failed");

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _userService.GetByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var valid = await _userService.ValidatePasswordAsync(user, dto.Password);
            if (!valid)
                return Unauthorized("Invalid credentials");

            var token = await _authService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
