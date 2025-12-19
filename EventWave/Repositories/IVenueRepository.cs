using EventWave.Models;

namespace EventWave.Repositories
{
    public interface IVenueRepository
    {
        Task<List<Venue>> GetAllAsync();
        Task<Venue?> GetByIdAsync(int id);
        Task<Venue> CreateAsync(Venue venue);
        Task<Venue?> UpdateAsync(Venue venue);
        Task<bool> DeleteAsync(int id);
    }
}
