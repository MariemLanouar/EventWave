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

            var duplicatedTypes = evt.TicketCapacities
                .GroupBy(tc => tc.TicketType)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatedTypes.Any())
            {
                throw new Exception(
                    "Duplicate ticket types are not allowed"
                );
            }
            evt.CreatedAt = DateTime.Now;
            evt.Status = EventStatus.Draft;

            
            foreach (var tc in evt.TicketCapacities)
            {
                tc.TicketsRemaining = tc.Capacity;
            }

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



        public async Task<List<Event>> GlobalSearchAsync(string keyword)
        {
            return await _eventRepository.GlobalSearchAsync(keyword);
        }

        public async Task<List<Event>> AdvancedSearchAsync(
            int? speakerId,
            string category,
            DateTime? start,
            string location,
            string description,
            string title)
        {
            return await _eventRepository.AdvancedSearchAsync(
                speakerId,
                category,
                start,
                location,
                description,
                title
            );
        }




    }

}
