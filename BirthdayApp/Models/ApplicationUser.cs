using Microsoft.AspNetCore.Identity;

namespace BirthdayApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false; // Admin must approve this!
    }
}