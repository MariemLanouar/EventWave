using EventWave.Repositories;
using EventWave.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationRepository repository;

        public RegistrationController(IRegistrationRepository repository)
        {
            this.repository = repository;
        }

        // GET api/registration
        [HttpGet]
        public async Task<IActionResult> GetRegistrations()
        {
            return Ok(await repository.GetRegistrations());
        }

        // GET api/registration/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistration(int id)
        {
            var reg = await repository.GetRegistration(id);
            if (reg == null)
                return NotFound("Registration not found");

            return Ok(reg);
        }

        // POST api/registration
        [HttpPost]
        public async Task<IActionResult> AddRegistration(RegistrationDTO dto)
        {
            var newReg = await repository.AddRegistration(dto);

            if (newReg != null)
            {
                return CreatedAtAction(nameof(GetRegistration),
                    new { id = newReg.Id },
                    newReg);
            }

            return BadRequest("Error creating registration");
        }

        // DELETE api/registration/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var result = await repository.DeleteRegistration(id);

            if (result)
                return NoContent();

            return BadRequest("Error deleting registration");
        }
    }
}
