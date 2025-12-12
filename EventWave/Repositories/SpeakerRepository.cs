using EventWave.Data;
using EventWave.Models;
using Microsoft.EntityFrameworkCore;

namespace EventWave.Repositories
{
    public class SpeakerRepository : ISpeakerRepository
    {
        private readonly ApplicationDBContext context;

        public SpeakerRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Speaker> AddAsync(Speaker speaker)
        {
            context.Speakers.Add(speaker);
            await context.SaveChangesAsync();
            return speaker;
        }

        public async Task<List<Speaker>> GetAllAsync()
        {
            return await context.Speakers
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }

        public async Task<Speaker> GetByIdAsync(int id)
        {
            return await context.Speakers
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Speaker> UpdateAsync(Speaker speaker)
        {
            var sp = await context.Speakers.FindAsync(speaker.Id);
            if (sp == null)
                return null;

            sp.Name = speaker.Name;
            sp.Bio = speaker.Bio;
            sp.Expertise = speaker.Expertise;
            sp.Contact = speaker.Contact;
            sp.ImageUrl = speaker.ImageUrl;
            sp.IsApproved = speaker.IsApproved;

            await context.SaveChangesAsync();
            return sp;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var speaker = await context.Speakers.FindAsync(id);
            if (speaker == null)
                return false;

            context.Speakers.Remove(speaker);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
