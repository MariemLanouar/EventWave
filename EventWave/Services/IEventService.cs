using EventWave.Models;
namespace EventWave.Services
{


    public interface IEventService
    {
        Task<Event> CreateEventAsync(Event evt);
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> UpdateEventAsync(Event evt);
        Task<bool> CancelEventAsync(int id);

        Task<List<Event>> GlobalSearchAsync(string keyword);
        Task<List<Event>> AdvancedSearchAsync(
            int? speakerId,
            string category,
            DateTime? start,
            string location,
            string description,
            string title
        );


    }
}