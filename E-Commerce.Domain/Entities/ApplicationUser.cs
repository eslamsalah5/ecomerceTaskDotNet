using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? LastLoginTime { get; set; }
    }
}