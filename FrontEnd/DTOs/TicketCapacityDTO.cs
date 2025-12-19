using FrontEnd.Models;

namespace FrontEnd.DTOs
{
    public class TicketCapacityDTO
    {
        public TicketType TicketType { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
    }
}
