using System.ComponentModel.DataAnnotations;

namespace EventWave.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       
    }
}
