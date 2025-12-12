using EventWave.DTOs;
using EventWave.Models;
using EventWave.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakerController : ControllerBase
    {
        private readonly ISpeakerService speakerService;

        public SpeakerController(ISpeakerService speakerService)
        {
            this.speakerService = speakerService;
        }

        // ➤ CREATE
        [HttpPost]
        public async Task<IActionResult> CreateSpeaker(SpeakerDTO spdto)
        {
            if (spdto == null)
                return BadRequest("Invalid data");

            var sp = new Speaker
            {
                Name = spdto.Name,
                Bio = spdto.Bio,
                Expertise = spdto.Expertise,
                Contact = spdto.Contact,
                ImageUrl = spdto.ImageUrl
            };

            var created = await speakerService.CreateSpeakerAsync(sp);
            return Ok(created);
        }

        // ➤ LIST
        [HttpGet]
        public async Task<IActionResult> GetSpeakers()
        {
            var speakers = await speakerService.GetAllSpeakersAsync();
            return Ok(speakers);
        }

        // ➤ GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sp = await speakerService.GetSpeakerByIdAsync(id);
            if (sp == null)
                return NotFound(new { message = "Speaker not found" });

            return Ok(sp);
        }

        // ➤ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpeaker(int id, Speaker sp)
        {
            if (id != sp.Id)
                return BadRequest(new { message = "ID mismatch" });

            var updated = await speakerService.UpdateSpeakerAsync(sp);
            if (updated == null)
                return NotFound(new { message = "Speaker not found" });

            return Ok(updated);
        }

        // ➤ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeaker(int id)
        {
            var deleted = await speakerService.DeleteSpeakerAsync(id);
            if (!deleted)
                return NotFound(new { message = "Speaker not found" });

            return Ok(new { message = "Speaker deleted successfully" });
        }
    }
}
