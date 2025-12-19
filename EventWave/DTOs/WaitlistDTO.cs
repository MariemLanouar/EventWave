using EventWave.Models;

namespace EventWave.DTOs
{
    public class WaitlistDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public TicketType TicketType { get; set; }
        public int TicketCount { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
