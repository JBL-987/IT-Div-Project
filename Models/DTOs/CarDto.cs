using System.ComponentModel.DataAnnotations; // Importing data annotations for validation

namespace RentalMobil.Models.DTOs // Namespace for Data Transfer Objects (DTOs) related to the RentalMobil application
{
    // CarDto class represents the data transfer object for car details
    public class CarDto
    {
        // Required property for the car make, with a custom error message for validation
        [Required(ErrorMessage = "Make is required.")]
        public string Make { get; set; }

        // Required property for the car model, with a custom error message for validation
        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }

        // Required property for the manufacturing year of the car, with a custom error message
        // and a range validation to ensure the year is realistic (between 1886 and 2100)
        [Required(ErrorMessage = "Year is required.")]
        [Range(1886, 2100, ErrorMessage = "Please enter a valid year.")]
        public int Year { get; set; }

        // Required property for the price of the car, with a custom error message
        // and a range validation to ensure the price is greater than zero
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }
}
