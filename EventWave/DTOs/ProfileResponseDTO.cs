namespace EventWave.DTOs
{
    public class ProfileResponseDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public string City { get; set; }
        public string? Bio { get; set; }

        public List<string> Roles { get; set; }
    }
}
