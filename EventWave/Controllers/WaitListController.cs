using EventWave.DTOs;
using EventWave.Repositories;
using EventWave.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitListController : ControllerBase
    {
        private readonly IWaitListService _waitListService;

        public WaitListController(IWaitListService waitListService)
        {
            _waitListService = waitListService;
        }

        // 🔹 Consulter toute la waitlist
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var waitlist = await _waitListService.GetAllAsync();
            var dtos = waitlist.Select(w => new WaitlistDTO
            {
                Id = w.Id,
                UserId = w.UserId,
                UserName = w.User?.FullName ?? "Unknown",
                UserEmail = w.User?.Email ?? "Unknown",
                EventId = w.EventId,
                EventTitle = w.Event?.Title ?? "Unknown",
                TicketType = w.TicketType,
                TicketCount = w.TicketCount,
                JoinedAt = w.CreatedAt
            });
            return Ok(dtos);
        }

        // 🔹 Consulter la waitlist d’un événement
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var waitlist = await _waitListService.GetByEventAsync(eventId);
            var dtos = waitlist.Select(w => new WaitlistDTO
            {
                Id = w.Id,
                UserId = w.UserId,
                UserName = w.User?.FullName ?? "Unknown",
                UserEmail = w.User?.Email ?? "Unknown",
                EventId = w.EventId,
                EventTitle = w.Event?.Title ?? "Unknown",
                TicketType = w.TicketType,
                TicketCount = w.TicketCount,
                JoinedAt = w.CreatedAt
            });
            return Ok(dtos);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _waitListService.ApproveAsync(id);
            if (!success) return NotFound();
            return Ok("Approved");
        }

        [HttpDelete("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _waitListService.RejectAsync(id);
            if (!success) return NotFound();
            return Ok("Rejected");
        }
    }
}
