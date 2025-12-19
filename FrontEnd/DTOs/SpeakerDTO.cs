namespace FrontEnd.DTOs
{
    public class SpeakerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Expertise { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
    }
}
