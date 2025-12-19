using EventWave.DTOs;
using EventWave.Repositories;
using Microsoft.AspNetCore.Mvc;
using EventWave.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [HttpGet("my-registrations")]
        [Authorize]
        public async Task<IActionResult> GetMyRegistrations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _service.GetRegistrationsByUserAsync(userId);
            return Ok(result);
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
        [Authorize]
        public async Task<IActionResult> AddRegistration(RegistrationDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            dto.UserId = userId;

            var newReg = await _service.AddRegistrationAsync(dto);
            dynamic reg = newReg;
            
            // Check properties dynamically if possible or trust structure
            // Using reflection to be safe since it's anonymous type or dynamic
            // But dynamic works if runtime type has properties.
            
            try 
            {
                if (reg.Status == "WAITLIST")
                    return Ok(newReg);
                if (reg.Status == "ERROR")
                    return BadRequest(newReg); // Changed to BadRequest for error
            }
            catch
            {
                // Fallback if property doesn't exist
            }

            return CreatedAtAction(nameof(GetRegistration),
                    new { id = reg.Id },
                    newReg);
            
        }

        //DELETE api/registration/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var result = await _service.DeleteRegistrationAsync(id);
            return Ok(new { message = result });
        }

    }
}
