namespace RentalMobil.Models.DTOs // Namespace for Data Transfer Objects (DTOs) related to the RentalMobil application
{
    // RentalRequestDto class represents the parameters required to request a car rental
    public class RentalRequestDto
    {
        // Unique identifier for the car being rented
        public int CarId { get; set; }

        // The date and time when the car rental starts
        public DateTime PickupDate { get; set; }

        // The date and time when the car rental ends
        public DateTime ReturnDate { get; set; }

        // Required email address of the renter, needed for communication and confirmation
        public required string RenterEmail { get; set; }
    }
}
