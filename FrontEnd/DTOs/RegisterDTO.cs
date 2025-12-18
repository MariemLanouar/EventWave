using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        //[StringLength ]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
