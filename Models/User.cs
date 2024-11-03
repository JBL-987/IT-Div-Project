namespace RentalMobil.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string LicenseNumber { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Auto-set to current UTC time
        public required string Status { get; set; }

        // Navigation property for rentals
        public required ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
