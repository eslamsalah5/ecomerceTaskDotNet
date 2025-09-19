using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Application.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        [RegularExpression(@"^(\+?20)?01[0125]\d{8}$", ErrorMessage = "Please enter a valid Egyptian phone number (e.g., 01012345678 or +201012345678)")]
        public string? PhoneNumber { get; set; }
    }
}