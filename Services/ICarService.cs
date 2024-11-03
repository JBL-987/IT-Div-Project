using RentalMobil.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalMobil.Services
{
    public interface ICarService
    {
        // Searches for cars based on specific criteria
        Task<IEnumerable<CarResponseDto>> SearchCarsAsync(CarSearchRequestDto searchRequest);

        // Retrieves a car by its ID
        Task<CarResponseDto> GetCarByIdAsync(int id);

        // Gets a list of unique manufacturing years for active cars
        Task<IEnumerable<int>> GetManufacturingYearsAsync();

        // Checks if a specific car is available for a given date range
        Task<bool> IsCarAvailableAsync(int carId, DateTime pickupDate, DateTime returnDate);

        // Retrieves all active cars
        Task<IEnumerable<CarResponseDto>> GetAllCarsAsync();
    }
}
