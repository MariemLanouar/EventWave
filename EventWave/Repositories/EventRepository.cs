using EventWave.Data;
using EventWave.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventWave.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDBContext _context;

        public EventRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Event> AddAsync(Event evt)
        {
            _context.Events.Add(evt);
            await _context.SaveChangesAsync();
            return evt;
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Speaker)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.Speaker)
                .Include(e => e.Registrations)
                .Include(e => e.Speakers)
                .OrderByDescending(e => e.Id)
                .ToListAsync();
        }

        public async Task<Event> UpdateAsync(Event evt)
        {
            var existingEvent = await _context.Events.FindAsync(evt.Id);
            if (existingEvent == null)
                return null;

            // Update properties
            existingEvent.Title = evt.Title;
            existingEvent.Description = evt.Description;
            existingEvent.Start = evt.Start;
            existingEvent.End = evt.End;
            existingEvent.Category = evt.Category;
            existingEvent.Location = evt.Location;
            existingEvent.Capacity = evt.Capacity;
            existingEvent.TicketsRemaining = evt.TicketsRemaining;
            existingEvent.Status = evt.Status;
            existingEvent.ImageUrl = evt.ImageUrl;
            existingEvent.SpeakerId = evt.SpeakerId;

            await _context.SaveChangesAsync();
            return existingEvent;
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var existingEvent = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (existingEvent == null)
                return false;

            // If tickets sold → refuse cancellation
            if (existingEvent.Registrations != null && existingEvent.Registrations.Count > 0)
                return false;

            existingEvent.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }




    }
}
