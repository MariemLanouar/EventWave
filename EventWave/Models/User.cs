using Microsoft.AspNetCore.Identity;

namespace EventWave.Models
{
    public class User: IdentityUser
    {
        public string FullName {  get; set; }
        public Profile Profile { get; set; } // Navigation
    }
}
