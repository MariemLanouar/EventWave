using EventWave.Models;

namespace EventWave.Repositories
{
    public interface IWaitListRepository
    {
        Task<List<WaitList>> GetAll();
        Task<List<WaitList>> GetByEvent(int eventId);
    }
}
