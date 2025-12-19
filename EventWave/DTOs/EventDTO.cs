using EventWave.Models;
using System.ComponentModel.DataAnnotations;

namespace EventWave.DTOs
{
    public class EventDTO
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int SpeakerId { get; set; }

        public string OrganizerId { get; set; }
        public string Category { get; set; }
        public int VenueId { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Draft;
        public string ImageUrl { get; set; }
        public List<TicketCapacityDTO> TicketCapacities { get; set; }
    }
}
