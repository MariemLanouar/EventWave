using FrontEnd.Models;

namespace FrontEnd.DTOs
{
    public class EventResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public EventStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public string OrganizerId { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        
        public int SpeakerId { get; set; }
        public string SpeakerName { get; set; }

        public List<TicketCapacityDTO> TicketCapacities { get; set; }
    }
}
