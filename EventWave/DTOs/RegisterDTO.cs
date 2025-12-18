using System.ComponentModel.DataAnnotations;

namespace EventWave.DTOs
{
    public class RegisterDTO
    {
        //[Required(ErrorMessage = "Name is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        //[StringLength ]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }

    }
}
