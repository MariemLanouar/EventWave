using EventWave.Models;
using EventWave.Repositories;
using EventWave.DTOs;

namespace EventWave.Services
{
    public class WaitListService : IWaitListService
    {
        private readonly IWaitListRepository _waitListRepository;
        private readonly IRegistrationRepository _registrationRepository;

        public WaitListService(IWaitListRepository waitListRepository, IRegistrationRepository registrationRepository)
        {
            _waitListRepository = waitListRepository;
            _registrationRepository = registrationRepository;
        }

        public async Task<List<WaitList>> GetAllAsync()
        {
            return await _waitListRepository.GetAll();
        }

        public async Task<List<WaitList>> GetByEventAsync(int eventId)
        {
            return await _waitListRepository.GetByEvent(eventId);
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var entry = await _waitListRepository.GetByIdAsync(id);
            if (entry == null) return false;

            var dto = new RegistrationDTO
            {
                UserId = entry.UserId,
                EventId = entry.EventId,
                TicketCount = entry.TicketCount,
                TicketType = entry.TicketType,
                PaymentMethod = entry.PaymentMethod
            };

            var result = await _registrationRepository.AddRegistration(dto);
            try
            {
                dynamic r = result;
                if (r.Status == "CONFIRMED")
                {
                    await _waitListRepository.DeleteAsync(id);
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public async Task<bool> RejectAsync(int id)
        {
            return await _waitListRepository.DeleteAsync(id);
        }
    }
}
