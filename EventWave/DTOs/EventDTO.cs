using System.ComponentModel.DataAnnotations;

namespace EventWave.DTOs
{
    public class EventDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int SpeakerId { get; set; }

        public string OrganizerId { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }

        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
    }
}
