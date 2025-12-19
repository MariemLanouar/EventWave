using EventWave.Models;

namespace EventWave.DTOs
{
    public class UpdateEventDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Category { get; set; }
        public int VenueId { get; set; }
        public string ImageUrl { get; set; }
    }
}
