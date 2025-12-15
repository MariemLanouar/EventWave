namespace EventWave.DTOs
{
    public class EventSalesDTO
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public int Capacity { get; set; }
        public int TicketsSold { get; set; }
        public bool IsSoldOut { get; set; }
        public double SoldPercentage { get; set; }
    }

}
