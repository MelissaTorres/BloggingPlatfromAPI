using Microsoft.AspNetCore.Identity;

namespace BloggingPlatformAPI.Models
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
