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
            return Ok(waitlist);
        }

        // 🔹 Consulter la waitlist d’un événement
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var waitlist = await _waitListService.GetByEventAsync(eventId);
            return Ok(waitlist);
        }
    }
}
