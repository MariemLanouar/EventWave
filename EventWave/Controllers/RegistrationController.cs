using EventWave.DTOs;
using EventWave.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _service;

        public RegistrationController(IRegistrationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegistrations()
        {
            return Ok(await _service.GetRegistrationsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistration(int id)
        {
            var reg = await _service.GetRegistrationAsync(id);
            if (reg == null)
                return NotFound("Registration not found");

            return Ok(reg);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegistration(RegistrationDTO dto)
        {
            var newReg = await _service.AddRegistrationAsync(dto);

            return CreatedAtAction(
                nameof(GetRegistration),
                new { id = newReg.Id },
                newReg
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var result = await _service.DeleteRegistrationAsync(id);

            if (!result)
                return NotFound("Registration not found");

            return NoContent();
        }
    }
}
