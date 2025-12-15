using EventWave.Models;

namespace EventWave.Repositories
{
    public interface IProfileRepository
    {
        Task<Profile?> GetByUserIdAsync(string userId);
        Task<Profile> CreateAsync(Profile profile);
        Task<Profile> UpdateAsync(Profile profile);
    }
}
