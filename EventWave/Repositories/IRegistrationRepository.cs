using EventWave.DTOs;
using EventWave.Models;

namespace EventWave.Repositories
{
    public interface IRegistrationRepository
    {

        Task<object> GetRegistrations();
        Task<object?> GetRegistration(int id);
        Task<Registration> AddRegistration(RegistrationDTO dto);
        Task<bool> DeleteRegistration(int id);

    }
}
