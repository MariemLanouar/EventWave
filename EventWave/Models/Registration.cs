namespace EventWave.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}
