using FrontEnd.Models;

namespace FrontEnd.DTOs
{
    public class RegistrationResponseDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public string EventImageUrl { get; set; } = string.Empty;
        public string EventVenue { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public List<TicketResponseDTO> Tickets { get; set; } = new();
    }

    public class TicketResponseDTO
    {
        public int Id { get; set; }
        public TicketType Type { get; set; }
        public decimal Price { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
    }
}
