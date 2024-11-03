namespace RentalMobil.Models // Namespace for models in the RentalMobil application
{
    // Rental class represents the properties of a car rental transaction
    public class Rental
    {
        // Unique identifier for the rental transaction
        public int Id { get; set; }

        // The start date and time of the rental period
        public DateTime RentalStartDate { get; set; }

        // The end date and time of the rental period
        public DateTime RentalEndDate { get; set; }

        // The model of the car being rented
        public string CarModel { get; set; }

        // The manufacturing year of the car being rented, represented as a string
        public string CarYear { get; set; }

        // The daily rental rate for the car
        public decimal DailyRate { get; set; }

        // Total number of days for which the car is rented
        public int TotalDays { get; set; }

        // Total amount charged for the rental, calculated based on the daily rate and total days
        public decimal TotalAmount { get; set; }

        // Indicates whether the rental payment has been completed (true) or not (false)
        public bool IsPaid { get; set; }
    }
}
