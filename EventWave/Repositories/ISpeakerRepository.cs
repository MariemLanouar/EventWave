using EventWave.Models;

namespace EventWave.Repositories
{
    public interface ISpeakerRepository
    {
        Task<Speaker> AddAsync(Speaker speaker);
        Task<List<Speaker>> GetAllAsync();
        Task<Speaker> GetByIdAsync(int id);
        Task<Speaker> UpdateAsync(Speaker speaker);
        Task<bool> DeleteAsync(int id);
    }
}
