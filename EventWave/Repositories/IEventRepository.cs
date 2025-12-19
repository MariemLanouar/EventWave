using EventWave.DTOs;
using EventWave.Models;
using System.Threading.Tasks;

namespace EventWave.Repositories
{
    public interface IEventRepository
    {
        public  Task<Event> AddAsync(Event evt);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetByOrganizerAsync(string organizerId);
        Task<Event?> GetByIdAsync(int id);
        Task<Event?> UpdateAsync(Event evt);
        Task<bool> DeleteEventAsync(int eventId);
        Task<List<Event>> GlobalSearchAsync(string keyword);
        Task<List<Event>> AdvancedSearchAsync(
        int? speakerId,
        string? category,
        DateTime? start,
        string? venueName,
        string? city,
        string? description,
        string? title);
        Task<OrganizerStatsDTO?> GetOrganizerStatsAsync(string organizerId);


    }
}
