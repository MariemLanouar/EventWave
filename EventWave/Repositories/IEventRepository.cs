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
        Task<List<Event>> GlobalSearchAsync(string keyword);
        Task<List<Event>> AdvancedSearchAsync(
            int? speakerId,
            string category,
            DateTime? start,
            string location,
            string description,
            string title
        );
        Task<OrganizerStatsDTO?> GetOrganizerStatsAsync(string organizerId);


    }
}
