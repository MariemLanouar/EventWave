using EventWave.Models;
using EventWave.Repositories;

namespace EventWave.Services
{
    public class WaitListService : IWaitListService
    {
        private readonly IWaitListRepository _waitListRepository;

        public WaitListService(IWaitListRepository waitListRepository)
        {
            _waitListRepository = waitListRepository;
        }

        public async Task<List<WaitList>> GetAllAsync()
        {
            return await _waitListRepository.GetAll();
        }

        public async Task<List<WaitList>> GetByEventAsync(int eventId)
        {
            return await _waitListRepository.GetByEvent(eventId);
        }
    }
}
