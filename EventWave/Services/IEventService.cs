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

    }
}