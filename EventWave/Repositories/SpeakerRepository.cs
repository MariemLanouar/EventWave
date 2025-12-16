using EventWave.Data;
using EventWave.DTOs;
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

        public async Task<Speaker?> GetByIdAsync(int id)
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

        public async Task<List<Speaker>> GetPendingSpeakersAsync()
        {
            return await context.Speakers
                .Where(s => !s.IsApproved)
                .ToListAsync();
        }

        public async Task<bool> ApproveSpeakerAsync(int id)
        {
            var speaker = await context.Speakers.FindAsync(id);
            if (speaker == null)
                return false;

            speaker.IsApproved = true;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RejectAsync(int id)
        {
            var speaker = await context.Speakers.FindAsync(id);
            if (speaker == null) return false;

            speaker.IsApproved = false;
            

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Speaker>> SearchAsync(string? search)
        {
            var query = context.Speakers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.Expertise.Contains(search)
                );
            }

            return await query.ToListAsync();
        }

        public async Task<SpeakerStatsDTO?> GetSpeakerStatsAsync(int speakerId)
        {
            var speaker = await context.Speakers
                .Where(s => s.Id == speakerId)
                .Select(s => new
                {
                    s.Id,
                    s.Name
                   
                })
                .FirstOrDefaultAsync();

            if (speaker == null)
                return null;

            var events = await context.Events
                .Where(e => e.SpeakerId == speakerId)
                .Select(e => new SpeakerEventStatsDTO
                {
                    EventId = e.Id,
                    Title = e.Title,
                    Capacity = e.Capacity,
                    TicketsSold = e.Registrations.Count(),
                    IsSoldOut = e.Registrations.Count() >= e.Capacity
                })
                .ToListAsync();

            return new SpeakerStatsDTO
            {
                SpeakerId = speaker.Id,
                FullName = speaker.Name,
                TotalEvents = events.Count,
                SoldOutEvents = events.Count(e => e.IsSoldOut),
                Events = events
            };
        }



    }
}
