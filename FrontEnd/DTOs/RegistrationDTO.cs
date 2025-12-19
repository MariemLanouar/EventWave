using FrontEnd.Models;
using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DTOs
{
    public class RegistrationDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        [Required]
        public int EventId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public TicketType TicketType { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "You can buy between 1 and 10 tickets.")]
        public int TicketCount { get; set; } = 1;
    }
}
