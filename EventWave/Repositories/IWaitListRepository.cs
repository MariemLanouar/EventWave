using EventWave.Models;

namespace EventWave.Repositories
{
    public interface IWaitListRepository
    {
        Task<List<WaitList>> GetAll();
        Task<List<WaitList>> GetByEvent(int eventId);
        Task<WaitList?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
