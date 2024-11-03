namespace RentalMobil.Models // Namespace for models in the RentalMobil application
{
    // Car class represents the properties of a car in the rental system
    public class Car
    {
        // Unique identifier for the car
        public int CarId { get; set; }

        // The type of car (e.g., SUV, sedan, hatchback), required field
        public required string CarType { get; set; }

        // The name of the car model, required field
        public required string CarName { get; set; }

        // The type of transmission (e.g., automatic, manual), required field
        public required string Transmission { get; set; }

        // The number of passengers that the car can accommodate
        public int PassengerCount { get; set; }

        // The year the car was manufactured
        public int ManufacturingYear { get; set; }

        // The daily rental rate for the car
        public decimal DailyRate { get; set; }

        // URL for the car's image, required field
        public required string ImageUrl { get; set; }

        // The current status of the car (e.g., 'active', 'inactive', 'rented'), required field
        public required string Status { get; set; }
    }
}
