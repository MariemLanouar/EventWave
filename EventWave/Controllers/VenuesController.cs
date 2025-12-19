using EventWave.Models;
using EventWave.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        // GET api/venues
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var venues = await _venueService.GetAllAsync();
            return Ok(venues);
        }

        // GET api/venues/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var venue = await _venueService.GetByIdAsync(id);
            if (venue == null)
                return NotFound();

            return Ok(venue);
        }

        // POST api/venues
        [HttpPost]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> Create(Venue venue)
        {
            var created = await _venueService.CreateAsync(venue);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/venues/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> Update(int id, Venue venue)
        {
            if (id != venue.Id)
                return BadRequest("ID mismatch");

            var updated = await _venueService.UpdateAsync(venue);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE api/venues/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _venueService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return Ok(new { message = "Venue deleted successfully" });
        }
    }
}
