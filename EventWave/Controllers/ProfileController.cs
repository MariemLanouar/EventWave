using EventWave.DTOs;
using EventWave.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventWave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        // GET api/profile/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var profile = await _profileService.GetMyProfileAsync(UserId);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        // POST api/profile
        [HttpPost]
        public async Task<IActionResult> CreateProfile(ProfileDTO dto)
        {
            await _profileService.CreateProfileAsync(UserId, dto);
            return Ok("Profile created");
        }

        // PUT api/profile
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(ProfileDTO dto)
        {
            await _profileService.UpdateProfileAsync(UserId, dto);
            return Ok("Profile updated");
        }
    }
}
