using EventWave.Data;
using EventWave.Models;
using Microsoft.EntityFrameworkCore;

namespace EventWave.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly ApplicationDBContext _context;

        public VenueRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Venue>> GetAllAsync()
        {
            return await _context.Venues
                .Include(v => v.Events)
                .ToListAsync();
        }

        public async Task<Venue?> GetByIdAsync(int id)
        {
            return await _context.Venues
                .Include(v => v.Events)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venue> CreateAsync(Venue venue)
        {
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            return venue;
        }

        public async Task<Venue?> UpdateAsync(Venue venue)
        {
            var existing = await _context.Venues.FindAsync(venue.Id);
            if (existing == null)
                return null;

            existing.Name = venue.Name;
            existing.Address = venue.Address;
            existing.City = venue.City;
            existing.Capacity = venue.Capacity;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return false;

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
