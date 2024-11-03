using System.ComponentModel.DataAnnotations;

namespace RentalMobil.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress] // Ensures valid email format
        public required string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 characters.")]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")] // Ensures Password and RePassword match
        public required string RePassword { get; set; }

        [Required]
        [Phone] // Validates phone number format
        public required string Phone { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 100 characters.")]
        public required string Address { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "License Number must be between 5 and 20 characters.")]
        public required string LicenseNumber { get; set; } // New property for license number
    }
}
