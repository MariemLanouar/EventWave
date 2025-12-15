using EventWave.DTOs;
using EventWave.Models;
using EventWave.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISpeakerService _speakerService;
        private readonly IEventService _eventService;

        public AdminController(IUserService userService, ISpeakerService speakerService, IEventService eventService)
        {
            _userService = userService;
            _speakerService = speakerService;
            _eventService = eventService;
        }




        [HttpPost("add-organizer")]
        public async Task<IActionResult> AddOrganizer([FromBody] RegisterDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                return BadRequest("Email and password are required.");

            var existingUser = await _userService.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest("A user with this email already exists.");

            var newOrg = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userService.AddUserAsync(newOrg, "Organizer", dto.Password);
            if (result==null)
                return BadRequest("Failed to create organizer.");

           

            return CreatedAtAction(null, new { newOrg.Id, newOrg.Email, newOrg.FullName });
        }

        [HttpGet("organizers")]
        public async Task<IActionResult> GetAllOrganizers()
        {
            var organizers = await _userService.GetUsersByRoleAsync("Organizer");

            return Ok(organizers.Select(o => new
            {
                o.Id,
                o.Email,
                o.FullName
            }));
        }


        [HttpDelete("organizers/{id}")]
        public async Task<IActionResult> RemoveOrganizer(string id)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(id);

                if (!success)
                    return NotFound("Organizer not found");

                return Ok("Organizer deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pending-speakers")]
        public async Task<IActionResult> GetPendingSpeakers()
        {
            var speakers = await _speakerService.GetPendingSpeakersAsync();
            return Ok(speakers);
        }

        [HttpPut("approve-speaker/{id}")]
        public async Task<IActionResult> ApproveSpeaker(int id)
        {
            var success = await _speakerService.ApproveSpeakerAsync(id);

            if (!success)
                return NotFound("Speaker not found");

            return Ok("Speaker approved successfully");
        }

        [HttpPut("reject-speaker/{id}")]
        public async Task<IActionResult> RejectSpeaker(int id)
        {
            var success = await _speakerService.RejectSpeakerAsync(id);

            if (!success)
                return NotFound("Speaker not found");

            return Ok("Speaker rejected successfully");
        }

        [HttpGet("organizers/{id}")]
        public async Task<IActionResult> GetOrganizerById(string id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound("Organizer not found");

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName
            });
        }


        [HttpGet("organizers/search")]
        public async Task<IActionResult> SearchOrganizers([FromQuery] string? search)
        {
            var organizers = await _userService.SearchUsersByRoleAsync("Organizer", search);

            return Ok(organizers.Select(o => new
            {
                o.Id,
                o.FullName,
                o.Email
            }));
        }

        [HttpGet("speakers/search")]
        public async Task<IActionResult> SearchSpeakers([FromQuery] string? search)
        {
            var speakers = await _speakerService.SearchSpeakersAsync(search);
            return Ok(speakers);
        }

        [HttpGet("organizers/{id}/stats")]
        public async Task<IActionResult> GetOrganizerStats(string id)
        {
            var stats = await _eventService.GetOrganizerStatsAsync(id);

            if (stats == null)
                return NotFound("Organizer not found");

            return Ok(stats);
        }

        [HttpGet("speakers/{id}/stats")]
        public async Task<IActionResult> GetSpeakerStats(int id)
        {
            var stats = await _speakerService.GetSpeakerStatsAsync(id);

            if (stats == null)
                return NotFound("Speaker not found");

            return Ok(stats);
        }



    }
}
