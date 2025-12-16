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
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int TicketsRemaining { get; set; }

        public string Status { get; set; } = "Draft";  // Draft, Published, Cancelled



        public string ImageUrl { get; set; }
        public List<Registration> Registrations { get; set; }
        //public List<WaitlistEntry> Waitlist { get; set; }
        public List<Speaker> Speakers { get; set; }
    }
}
