using EventWave.Data;
using EventWave.Models;
using Microsoft.EntityFrameworkCore;

namespace EventWave.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDBContext context;

        public ProfileRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Profile?> GetByUserIdAsync(string userId)
        {
            return await context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Profile> CreateAsync(Profile profile)
        {
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();
            return profile;
        }

        public async Task<Profile> UpdateAsync(Profile profile)
        {
            context.Profiles.Update(profile);
            await context.SaveChangesAsync();
            return profile;
        }
    }
}
