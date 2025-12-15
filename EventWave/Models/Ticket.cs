using System.Net.Sockets;

namespace EventWave.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }

        // Relation : un ticket appartient à un Registration
        public int? RegistrationId { get; set; }
        public Registration Registration { get; set; }

        // Type du ticket : VIP, Regular, Student, etc.
        public TicketType Type { get; set; }

        // Numéro unique du ticket
        public string TicketNumber { get; set; }

        // Prix du ticket
        public decimal Price { get; set; }

        //public string QrCode { get; set; }


        
    }
}
