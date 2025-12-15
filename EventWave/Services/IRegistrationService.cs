using EventWave.DTOs;
using EventWave.Models;

namespace EventWave.Services
{
    public interface IRegistrationService
    {
        Task<object> GetRegistrationsAsync();
        Task<object?> GetRegistrationAsync(int id);
        Task<Registration> AddRegistrationAsync(RegistrationDTO dto);
        Task<bool> DeleteRegistrationAsync(int id);

    }
}
