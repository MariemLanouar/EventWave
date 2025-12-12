using System.ComponentModel.DataAnnotations;

namespace EventWave.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        //[StringLength ]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "confirming password is required.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
