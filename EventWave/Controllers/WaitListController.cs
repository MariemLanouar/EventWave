using EventWave.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitListController : ControllerBase
    {
        private readonly IWaitListRepository _waitListRepository;

        public WaitListController(IWaitListRepository waitListRepository)
        {
            _waitListRepository = waitListRepository;
        }

        // 🔹 Consulter toute la waitlist
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var waitlist = await _waitListRepository.GetAll();
            return Ok(waitlist);
        }

        // 🔹 Consulter la waitlist d’un événement
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var waitlist = await _waitListRepository.GetByEvent(eventId);
            return Ok(waitlist);
        }
    }
}
