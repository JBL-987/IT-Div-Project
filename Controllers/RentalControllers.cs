using Microsoft.AspNetCore.Mvc; // Importing the MVC framework for building web applications
using RentalMobil.Models; // Importing the models used in the application

namespace RentalMobil.Controllers // Declaring the namespace for the RentalController
{
    // Defining the RentalController class which inherits from Controller
    public class RentalController : Controller
    {
        // Action method to display the rental history
        public IActionResult History()
        {
            // Creating a list of rentals to simulate rental history data
            var rentals = new List<Rental>
            {
                // Adding the first rental entry to the list
                new Rental
                {
                    RentalStartDate = new DateTime(2024, 10, 16), // Start date of the rental
                    RentalEndDate = new DateTime(2024, 10, 19),   // End date of the rental
                    CarModel = "Suzuki Ertiga",                     // Model of the rented car
                    CarYear = "2019",                               // Year of the car
                    DailyRate = 450000,                             // Daily rental rate
                    TotalDays = 4,                                  // Total days rented
                    TotalAmount = 1800000,                          // Total amount for the rental
                    IsPaid = true                                   // Payment status
                },
                // Adding the second rental entry to the list
                new Rental
                {
                    RentalStartDate = new DateTime(2024, 9, 10),  // Start date of the rental
                    RentalEndDate = new DateTime(2024, 9, 12),    // End date of the rental
                    CarModel = "Honda Brio",                        // Model of the rented car
                    CarYear = "2020",                               // Year of the car
                    DailyRate = 300000,                             // Daily rental rate
                    TotalDays = 3,                                  // Total days rented
                    TotalAmount = 900000,                           // Total amount for the rental
                    IsPaid = false                                  // Payment status
                },
                // Adding the third rental entry to the list
                new Rental
                {
                    RentalStartDate = new DateTime(2024, 8, 25),  // Start date of the rental
                    RentalEndDate = new DateTime(2024, 8, 28),    // End date of the rental
                    CarModel = "Toyota Avanza",                     // Model of the rented car
                    CarYear = "2021",                               // Year of the car
                    DailyRate = 500000,                             // Daily rental rate
                    TotalDays = 4,                                  // Total days rented
                    TotalAmount = 2000000,                          // Total amount for the rental
                    IsPaid = true                                   // Payment status
                }
            };

            // Returning the view with the list of rentals to display the rental history
            return View(rentals);
        }
    }
}
