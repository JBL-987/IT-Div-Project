using System.ComponentModel.DataAnnotations;

namespace RentalMobil.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress] // Ensures the input is a valid email format
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] // Specifies that this is a password field
        public required string Password { get; set; }
    }
}
