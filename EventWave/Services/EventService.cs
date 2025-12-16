namespace EventWave.Services
{
    using EventWave.DTOs;
    using EventWave.Models;
    using EventWave.Repositories;

    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Event> CreateEventAsync(Event evt)
        {
            evt.CreatedAt = DateTime.Now;
            evt.Status = "Draft";
            evt.TicketsRemaining = evt.Capacity;

            return await _eventRepository.AddAsync(evt);
        }

       

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return (List<Event>)await _eventRepository.GetAllAsync();
        }


        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }

        public async Task<Event> UpdateEventAsync(Event evt)
        {
            return await _eventRepository.UpdateAsync(evt);
        }

        public async Task<bool> CancelEventAsync(int id)
        {
            return await _eventRepository.DeleteEventAsync(id);
        }

        public async Task<OrganizerStatsDTO?> GetOrganizerStatsAsync(string organizerId)
        {
            return await _eventRepository.GetOrganizerStatsAsync(organizerId);
        }



    }

}
