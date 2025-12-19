using EventWave.Models;

namespace EventWave.Services
{
    public interface IWaitListService
    {
        Task<List<WaitList>> GetAllAsync();
        Task<List<WaitList>> GetByEventAsync(int eventId);
        Task<bool> ApproveAsync(int id);
        Task<bool> RejectAsync(int id);
    }
}
