using EventWave.Models;
using EventWave.Repositories;

namespace EventWave.Services
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepo;

        public VenueService(IVenueRepository venueRepo)
        {
            _venueRepo = venueRepo;
        }

        public Task<List<Venue>> GetAllAsync()
            => _venueRepo.GetAllAsync();

        public Task<Venue?> GetByIdAsync(int id)
            => _venueRepo.GetByIdAsync(id);

        public Task<Venue> CreateAsync(Venue venue)
            => _venueRepo.CreateAsync(venue);

        public Task<Venue?> UpdateAsync(Venue venue)
            => _venueRepo.UpdateAsync(venue);

        public Task<bool> DeleteAsync(int id)
            => _venueRepo.DeleteAsync(id);
    }
}
