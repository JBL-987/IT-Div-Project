namespace RentalMobil.Models.DTOs // Namespace for Data Transfer Objects (DTOs) related to the RentalMobil application
{
    // CarSearchRequestDto class represents the search request parameters for car searches
    public class CarSearchRequestDto
    {
        // Optional property for the pickup date of the car rental
        public DateTime? PickupDate { get; set; }

        // Optional property for the return date of the car rental
        public DateTime? ReturnDate { get; set; }

        // Optional property for filtering cars by manufacturing year
        public int? ManufacturingYear { get; set; }

        // Optional property for specifying the column to sort the search results
        public string? SortBy { get; set; }

        // Optional property for specifying the sort direction (e.g., 'ASC' or 'DESC')
        public string? SortDirection { get; set; }
    }
}
