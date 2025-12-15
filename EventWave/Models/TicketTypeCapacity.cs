namespace EventWave.Models
{
    public class TicketTypeCapacity
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        

        public TicketType TicketType { get; set; }

        public int Capacity { get; set; }
        public int TicketsRemaining { get; set; }

        public decimal Price { get; set; }
    }

}
