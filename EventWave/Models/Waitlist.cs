namespace EventWave.Models
{
    public class WaitList
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public int EventId { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }

        public int TicketCount { get; set; }
        public TicketType TicketType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
