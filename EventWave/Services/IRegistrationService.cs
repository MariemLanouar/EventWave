using EventWave.DTOs;
using EventWave.Models;

namespace EventWave.Services
{
    public interface IRegistrationService
    {
        Task<object> GetRegistrationsAsync();
        Task<object?> GetRegistrationAsync(int id);
        Task<object> AddRegistrationAsync(RegistrationDTO dto);
        Task<string> DeleteRegistrationAsync(int id);

    }
}
