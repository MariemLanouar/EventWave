using EventWave.DTOs;
using EventWave.Models;

namespace EventWave.Repositories
{
    public interface IEventRepository
    {
        public  Task<Event> AddAsync(Event evt);
        public  Task<Event> GetByIdAsync(int id);
        public Task<List<Event>> GetAllAsync();
        Task<Event> UpdateAsync(Event evt);
        Task<bool> DeleteEventAsync(int eventId);
        Task<OrganizerStatsDTO?> GetOrganizerStatsAsync(string organizerId);


    }
}
