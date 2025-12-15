using EventWave.DTOs;
using EventWave.Repositories;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var newReg = await repository.AddRegistration(dto);
            dynamic reg = newReg;
            if (reg.Status == "WAITLIST")
                return Ok(newReg);
            if (reg.Status == "ERROR")
                return Ok(newReg);
            


            return CreatedAtAction(nameof(GetRegistration),
                    new { id = reg.Id },
                    newReg);
            
        }

        //DELETE api/registration/{id}
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var message = await repository.DeleteRegistration(id);

            if (message == "Cette réservation n’a pas été trouvée.")
                return NotFound(message);

            return Ok(new { message });
        }

    }
}
