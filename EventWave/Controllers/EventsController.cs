using EventWave.Data;
using EventWave.DTOs;
using EventWave.Models;
using EventWave.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EventWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid event data.");

            var evt = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Start = dto.Start,
                End = dto.End,
                Category = dto.Category,
                SpeakerId = dto.SpeakerId,
                OrganizerId = dto.OrganizerId,
                VenueId = dto.VenueId,
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,

                TicketCapacities = dto.TicketCapacities?.Select(tc => new TicketTypeCapacity
                {
                    TicketType = tc.TicketType,
                    Capacity = tc.Capacity,
                    TicketsRemaining = tc.Capacity,
                    Price = tc.Price
                }).ToList() ?? new List<TicketTypeCapacity>()
            };

            try
            {
                var createdEvent = await _eventService.CreateEventAsync(evt);
                return CreatedAtAction(nameof(GetById), new { id = createdEvent.Id }, MapToResponseDTO(createdEvent));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            var dtos = events.Select(MapToResponseDTO).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evt = await _eventService.GetEventByIdAsync(id);
            if (evt == null)
            {
                return NotFound(new { message = "Event not found" });
            }
            return Ok(MapToResponseDTO(evt));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDTO dto)
        {
            var updated = await _eventService.UpdateEventAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = "Event not found" });

            return Ok(MapToResponseDTO(updated));
        }

        [HttpPatch("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelEvent(int id)
        {
            var result = await _eventService.CancelEventAsync(id);

            if (!result)
                return BadRequest(new { message = "Event cannot be cancelled (either it does not exist or tickets have been sold)." });

            return Ok(new { message = "Event cancelled successfully." });
        }

        [HttpGet("my-events")]
        [Authorize]
        public async Task<IActionResult> GetMyEvents()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var events = await _eventService.GetEventsByOrganizerAsync(userId);
            var dtos = events.Select(MapToResponseDTO).ToList();
            return Ok(dtos);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GlobalSearch(string q)
        {
            var results = await _eventService.GlobalSearchAsync(q);
            var dtos = results.Select(MapToResponseDTO).ToList();
            return Ok(dtos);
        }

        private EventResponseDTO MapToResponseDTO(Event e)
        {
            return new EventResponseDTO
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Start = e.Start,
                End = e.End,
                Category = e.Category,
                ImageUrl = e.ImageUrl,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                OrganizerId = e.OrganizerId,
                VenueId = e.VenueId,
                VenueName = e.Venue?.Name,
                SpeakerId = e.SpeakerId,
                SpeakerName = e.Speaker?.Name, // Assuming Speaker has Name property
                TicketCapacities = e.TicketCapacities?.Select(tc => new TicketCapacityDTO
                {
                    TicketType = tc.TicketType,
                    Capacity = tc.Capacity,
                    Price = tc.Price
                }).ToList() ?? new List<TicketCapacityDTO>()
            };
        }
    }
}
