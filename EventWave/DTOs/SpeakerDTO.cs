using System.ComponentModel.DataAnnotations;

namespace EventWave.DTOs
{
    public class SpeakerDTO
    {
        [Required(ErrorMessage = "Name is required!!!")]
        [MinLength(3, ErrorMessage = "Name must contain at least 3 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bio is required")]
        [MinLength(10, ErrorMessage = "Bio must contain at least 10 characters!!!")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Expertise is required")]
        public string Expertise { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Contact { get; set; }

        public string ImageUrl { get; set; }
    }
}
