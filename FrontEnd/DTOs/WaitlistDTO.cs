using FrontEnd.Models;

namespace FrontEnd.DTOs
{
    public class WaitlistDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty; // Assuming backend returns this
        public string UserEmail { get; set; } = string.Empty;
        public int EventId { get; set; }
        public TicketType TicketType { get; set; }
        public int TicketCount { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
