using Microsoft.AspNetCore.Identity;

namespace EventWave.Models
{
    public class User: IdentityUser
    {
        public string FullName {  get; set; }
    }
}
