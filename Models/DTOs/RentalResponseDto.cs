namespace RentalMobil.Models.DTOs // Namespace for Data Transfer Objects (DTOs) related to the RentalMobil application
{
    // RentalResponseDto class represents the response data for a rental transaction
    public class RentalResponseDto
    {
        // Unique identifier for the rental
        public int RentalId { get; set; }

        // Details of the car being rented, required for the rental response
        public required CarResponseDto CarDetails { get; set; }

        // The date and time when the car rental starts
        public required DateTime PickupDate { get; set; }

        // The date and time when the car rental ends
        public required DateTime ReturnDate { get; set; }

        // The total price for the rental, calculated based on the rental duration and daily rate
        public required decimal TotalPrice { get; set; }

        // The current status of the rental (e.g., 'active', 'completed', 'cancelled')
        public required string Status { get; set; }
    }
}
