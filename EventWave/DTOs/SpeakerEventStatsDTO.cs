namespace EventWave.DTOs
{
    public class SpeakerEventStatsDTO
    {
        public int EventId { get; set; }
        public string Title { get; set; }

        public int Capacity { get; set; }
        public int TicketsSold { get; set; }
        public bool IsSoldOut { get; set; }
    }
}
