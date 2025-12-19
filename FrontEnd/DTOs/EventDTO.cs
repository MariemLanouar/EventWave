using System.ComponentModel.DataAnnotations;
using FrontEnd.Models;

namespace FrontEnd.DTOs
{
    public class EventDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now.AddHours(2);

        public int SpeakerId { get; set; }

        public string OrganizerId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Venue is required.")]
        public int VenueId { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Draft;
        public string ImageUrl { get; set; } = string.Empty;

        public List<TicketCapacityDTO> TicketCapacities { get; set; } = new();
    }
}
