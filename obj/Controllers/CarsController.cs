using Microsoft.AspNetCore.Mvc;
using RentalMobil.Services;
using RentalMobil.Models;
using RentalMobil.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace RentMobil.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
        private readonly IRentalService _rentalService;

        public CarsController(ICarService carService, IRentalService rentalService)
        {
            _carService = carService;
            _rentalService = rentalService;
        }

        // API endpoint for searching cars
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CarResponseDto>>> SearchCars([FromQuery] CarSearchRequestDto searchRequest)
        {
            try
            {
                var cars = await _carService.SearchCarsAsync(searchRequest);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving cars", error = ex.Message });
            }
        }

        // API endpoint for getting car details by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CarResponseDto>> GetCarById(int id)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(id);
                if (car == null)
                    return NotFound(new { message = "Car not found" });

                return Ok(car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving car", error = ex.Message });
            }
        }

        // API endpoint for creating a rental
        [HttpPost("rent")]
        public async Task<ActionResult> RentCar([FromBody] RentalRequestDto rentalRequest)
        {
            try
            {
                var result = await _rentalService.CreateRentalAsync(rentalRequest);
                return Ok(new { message = "Rental created successfully", data = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating rental", error = ex.Message });
            }
        }

        // In CarsController.cs
        [HttpGet("List")]
        public async Task<IActionResult> List([FromQuery] CarSearchRequestDto searchRequest)
        {
            try
            {
                var cars = await _carService.SearchCarsAsync(searchRequest);
                var carList = cars.ToList(); // Ensure it's a list for the view
                return View(carList); // Passes the list of cars to the "List.cshtml" view in the Views/Cars folder
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving cars", error = ex.Message });
            }
        }

    }
}
