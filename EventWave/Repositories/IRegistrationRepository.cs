using EventWave.DTOs;
using EventWave.Models;

namespace EventWave.Repositories
{
    public interface IRegistrationRepository
    {

        Task<object?> GetRegistrations();
        Task<object?> GetRegistration(int id);
        Task<object> AddRegistration(RegistrationDTO dto);
        Task<string> DeleteRegistration(int id);

    }
}
