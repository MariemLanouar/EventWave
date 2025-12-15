using EventWave.Models;

public class TicketDto
{
    public int Id { get; set; }
    public TicketType Type { get; set; }
    public decimal Price { get; set; }
    public string TicketNumber { get; set; }
}
