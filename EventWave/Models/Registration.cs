namespace EventWave.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
        // Ajout d’un ticket lié à l'inscription
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
