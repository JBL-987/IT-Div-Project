namespace RentalMobil.Models.DTOs // Namespace for Data Transfer Objects (DTOs) related to the RentalMobil application
{
    // UserResponseDto class represents the response data for user information
    public class UserResponseDto
    {
        // Unique identifier for the user
        public int UserId { get; set; }

        // Required email address of the user, essential for user identification and communication
        public required string Email { get; set; }

        // Required first name of the user
        public required string FirstName { get; set; }

        // Required last name of the user
        public required string LastName { get; set; }

        // Required phone number of the user for contact purposes
        public required string PhoneNumber { get; set; }

        // Required status of the user (e.g., 'active', 'inactive'), indicating the user's current state
        public required string Status { get; set; }
    }
}
