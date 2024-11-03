using Microsoft.AspNetCore.Mvc; // Importing the MVC framework for building web applications
using RentalMobil.Models.DTOs; // Importing Data Transfer Objects (DTOs) for car data
using RentalMobil.Services; // Importing services for business logic related to cars
using System.Collections.Generic; // Importing generic collections for handling lists
using System.Threading.Tasks; // Importing tasks for asynchronous programming

namespace RentalMobil.Controllers // Declaring the namespace for the controller
{
    // Defining the CarControllers class which inherits from Controller
    public class CarControllers : Controller
    {
        // Dependency injection of the car service interface
        private readonly ICarService _carService;

        // Constructor to initialize the car service
        public CarControllers(ICarService carService)
        {
            _carService = carService; // Assigning the injected service to the private field
        }

        // Action method to list all cars
        public async Task<IActionResult> List()
        {
            // Asynchronously retrieve the list of cars from the service
            var cars = await _carService.GetAllCarsAsync();
            return View(cars); // Return the view with the list of cars
        }

        // Action method to display details of a specific car
        public async Task<IActionResult> Details(int id)
        {
            // Asynchronously retrieve a car by its ID from the service
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) // Check if the car was found
            {
                return NotFound(); // Return a 404 Not Found response if not found
            }
            return View(car); // Return the view with the car details
        }

        // Action method to create a new car
        [HttpPost] // Specify that this method handles POST requests
        [ValidateAntiForgeryToken] // Enable protection against cross-site request forgery
        public async Task<IActionResult> Create(CarDto carDto)
        {
            // Check if the model state is valid (all required fields are filled correctly)
            if (ModelState.IsValid)
            {
                // Asynchronously add the new car using the service
                await _carService.AddCarAsync(carDto);
                return RedirectToAction(nameof(List)); // Redirect to the List action after successful creation
            }
            return View(carDto); // If the model is invalid, return the same view with the car data to correct it
        }
    }
}
