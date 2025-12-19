
using EventWave.DTOs;
using EventWave.Models;
using EventWave.Repositories;
using global::EventWave.DTOs;
using global::EventWave.Models;
using global::EventWave.Repositories;

namespace EventWave.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _repository;

        public RegistrationService(IRegistrationRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> GetRegistrationsAsync()
        {
            return await _repository.GetRegistrations();
        }

        public async Task<object?> GetRegistrationsByUserAsync(string userId)
        {
            return await _repository.GetRegistrationsByUser(userId);
        }

        public async Task<object?> GetRegistrationAsync(int id)
        {
            return await _repository.GetRegistration(id);
        }

        public async Task<object> AddRegistrationAsync(RegistrationDTO dto)
        {
            
            return await _repository.AddRegistration(dto);
        }

        public async Task<string> DeleteRegistrationAsync(int id)
        {
            return await _repository.DeleteRegistration(id);
        }
    }
}
