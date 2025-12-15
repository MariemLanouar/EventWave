namespace EventWave.Models
{
    public class Profile
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Phone { get; set; }
        public string City { get; set; }

        // Utilisé UNIQUEMENT si Organizer
        public string? Bio { get; set; }
    }
}
