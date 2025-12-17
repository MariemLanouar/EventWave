namespace EventWave.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

   
        public string OrganizerId { get; set; }

        public string Category { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; }



        public EventStatus Status { get; set; } = EventStatus.Draft;



        public string ImageUrl { get; set; }

        public List<TicketTypeCapacity> TicketCapacities { get; set; }
        public List<Registration> Registrations { get; set; }
        
        public List<Speaker> Speakers { get; set; }
    }
}
