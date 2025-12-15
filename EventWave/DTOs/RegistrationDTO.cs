using EventWave.Models;

public class RegistrationDTO
{
    // USER
    public string UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    // EVENT
    public int EventId { get; set; }

    // PAYMENT
    public PaymentMethod PaymentMethod { get; set; }

    // TICKETS
    public TicketType TicketType { get; set; }
    public int TicketCount { get; set; } // nombre de tickets

    // Total calculé par backend
}
