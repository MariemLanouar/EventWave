using EventWave.Data;
using EventWave.DTOs;
using EventWave.Models;
using EventWave.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> CreateEvent([FromBody] EventDTO evtdto)
        {
            if (evtdto == null)
                return BadRequest("Invalid data.");

            var evt = new Event
            {
                Title = evtdto.Title,
                Description = evtdto.Description,
                Start = evtdto.Start,
                End = evtdto.End,
                Category = evtdto.Category,
                Location = evtdto.Location,
                
                SpeakerId = evtdto.SpeakerId,
                ImageUrl = evtdto.ImageUrl,
                OrganizerId = evtdto.OrganizerId,
                CreatedAt = DateTime.Now,
                Status = evtdto.Status,

                TicketCapacities = evtdto.TicketCapacities.Select(tc => new TicketTypeCapacity
                {
                    TicketType = tc.TicketType,
                    Capacity = tc.Capacity,
                    TicketsRemaining = tc.Capacity,
                    Price = tc.Price
                }).ToList()
            };

            var created = await _eventService.CreateEventAsync(evt);
            return Ok(created);
        }
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evt = await _eventService.GetEventByIdAsync(id);
            if (evt == null)
            {
                return NotFound(new { message = "Event not found" });
            }
            return Ok(evt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event evt)
        {
            if (id != evt.Id)
                return BadRequest(new { message = "Event ID mismatch" });

            var updated = await _eventService.UpdateEventAsync(evt);
            if (updated == null)
                return NotFound(new { message = "Event not found" });

            return Ok(updated);
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> CancelEvent(int id)
        {
            var result = await _eventService.CancelEventAsync(id);

            if (!result)
                return BadRequest(new { message = "Event cannot be cancelled (either it does not exist or tickets have been sold)." });

            return Ok(new { message = "Event cancelled successfully." });
        }


    }
}
