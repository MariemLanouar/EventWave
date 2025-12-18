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
        private readonly IProfileService _profileService;

        public AuthController(IUserService userService, IAuthService authService, IProfileService profileService)
        {
            _userService = userService;
            _authService = authService;
            _profileService = profileService;
        }

   
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var createdUser = await _userService.AddUserAsync(user, "Client", dto.Password);
            if (createdUser == null)
                return BadRequest("Registration failed");

            // ✅ Création automatique du Profile
            var profile = new Profile
            {
                UserId = createdUser.Id,
                Phone = dto.PhoneNumber,
                City = dto.City,
                Bio = null
            };

            await _profileService.CreateProfileInternalAsync(profile);

            return Ok("User registered successfully");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
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
