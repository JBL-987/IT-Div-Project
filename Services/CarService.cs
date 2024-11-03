using RentalMobil.Data; // Namespace for database connection
using RentalMobil.Models.DTOs; // Namespace for Data Transfer Objects (DTOs)
using Microsoft.Data.SqlClient; // Namespace for SQL Server client
using Microsoft.Extensions.Logging; // Namespace for logging
using System; // Namespace for basic system functionality
using System.Collections.Generic; // Namespace for generic collections
using System.Linq; // Namespace for LINQ operations
using System.Threading.Tasks; // Namespace for asynchronous programming

namespace RentalMobil.Services
{
    // CarService class implementing the ICarService interface
    public class CarService : ICarService
    {
        private readonly DatabaseConnection _db; // Database connection instance
        private readonly ILogger<CarService> _logger; // Logger instance for logging errors

        // Constructor to initialize DatabaseConnection and ILogger
        public CarService(DatabaseConnection db, ILogger<CarService> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Asynchronous method to search for cars based on the given search criteria
        public async Task<IEnumerable<CarResponseDto>> SearchCarsAsync(CarSearchRequestDto searchRequest)
        {
            try
            {
                using var conn = _db.GetConnection(); // Get a new database connection
                await conn.OpenAsync(); // Open the connection asynchronously

                // SQL query to select cars based on search criteria and availability
                var query = @"
                    SELECT 
                        c.*,
                        CASE 
                            WHEN EXISTS (
                                SELECT 1 FROM rentals r 
                                WHERE r.car_id = c.car_id 
                                AND r.status NOT IN ('cancelled', 'completed')
                                AND (
                                    (@PickupDate BETWEEN r.pickup_date AND r.return_date)
                                    OR (@ReturnDate BETWEEN r.pickup_date AND r.return_date)
                                    OR (r.pickup_date BETWEEN @PickupDate AND @ReturnDate)
                                )
                            ) THEN 0 ELSE 1 END as is_available
                    FROM cars c
                    WHERE (@ManufacturingYear IS NULL OR c.manufacturing_year = @ManufacturingYear)
                    AND c.status = 'active'";

                // Check for sorting options
                if (!string.IsNullOrEmpty(searchRequest.SortBy))
                {
                    // Allowed columns for sorting
                    var allowedSortColumns = new[] { "daily_rate", "manufacturing_year", "car_name" };
                    // Determine the sort column
                    var sortBy = allowedSortColumns.Contains(searchRequest.SortBy.ToLower())
                        ? searchRequest.SortBy
                        : "daily_rate"; // Default sorting by daily rate

                    // Determine sort direction
                    var sortDirection = searchRequest.SortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";
                    query += $" ORDER BY {sortBy} {sortDirection}"; // Append ORDER BY clause
                }

                using var cmd = new SqlCommand(query, conn); // Create SQL command
                // Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@PickupDate", (object)searchRequest.PickupDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ReturnDate", (object)searchRequest.ReturnDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ManufacturingYear", (object)searchRequest.ManufacturingYear ?? DBNull.Value);

                var cars = new List<CarResponseDto>(); // List to hold car response DTOs
                using var reader = await cmd.ExecuteReaderAsync(); // Execute query and read results asynchronously

                // Loop through the results
                while (await reader.ReadAsync())
                {
                    // Check if the car is available
                    if (reader.GetInt32(reader.GetOrdinal("is_available")) == 1)
                    {
                        // Add available car to the list
                        cars.Add(new CarResponseDto
                        {
                            CarId = reader.GetInt32(reader.GetOrdinal("car_id")),
                            CarType = reader.GetString(reader.GetOrdinal("car_type")),
                            CarName = reader.GetString(reader.GetOrdinal("car_name")),
                            Transmission = reader.GetString(reader.GetOrdinal("transmission")),
                            PassengerCount = reader.GetInt32(reader.GetOrdinal("passenger_count")),
                            ManufacturingYear = reader.GetInt32(reader.GetOrdinal("manufacturing_year")),
                            DailyRate = reader.GetDecimal(reader.GetOrdinal("daily_rate")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("image_url"))
                        });
                    }
                }

                return cars; // Return the list of available cars
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching cars"); // Log the error
                throw; // Rethrow the exception
            }
        }

        // Asynchronous method to get details of a car by its ID
        public async Task<CarResponseDto> GetCarByIdAsync(int id)
        {
            try
            {
                using var conn = _db.GetConnection(); // Get a new database connection
                await conn.OpenAsync(); // Open the connection asynchronously

                // SQL query to select a car by ID
                var query = "SELECT * FROM cars WHERE car_id = @CarId AND status = 'active'";
                using var cmd = new SqlCommand(query, conn); // Create SQL command
                cmd.Parameters.AddWithValue("@CarId", id); // Add car ID parameter

                using var reader = await cmd.ExecuteReaderAsync(); // Execute query and read results asynchronously
                if (await reader.ReadAsync()) // If a car is found
                {
                    // Map the data to CarResponseDto
                    return new CarResponseDto
                    {
                        CarId = reader.GetInt32(reader.GetOrdinal("car_id")),
                        CarType = reader.GetString(reader.GetOrdinal("car_type")),
                        CarName = reader.GetString(reader.GetOrdinal("car_name")),
                        Transmission = reader.GetString(reader.GetOrdinal("transmission")),
                        PassengerCount = reader.GetInt32(reader.GetOrdinal("passenger_count")),
                        ManufacturingYear = reader.GetInt32(reader.GetOrdinal("manufacturing_year")),
                        DailyRate = reader.GetDecimal(reader.GetOrdinal("daily_rate")),
                        ImageUrl = reader.GetString(reader.GetOrdinal("image_url"))
                    };
                }

                return null; // Return null if no car is found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting car by id {CarId}", id); // Log the error
                throw; // Rethrow the exception
            }
        }

        // Asynchronous method to get distinct manufacturing years of available cars
        public async Task<IEnumerable<int>> GetManufacturingYearsAsync()
        {
            try
            {
                using var conn = _db.GetConnection(); // Get a new database connection
                await conn.OpenAsync(); // Open the connection asynchronously

                // SQL query to select distinct manufacturing years
                var query = "SELECT DISTINCT manufacturing_year FROM cars WHERE status = 'active' ORDER BY manufacturing_year DESC";
                using var cmd = new SqlCommand(query, conn); // Create SQL command

                var years = new List<int>(); // List to hold distinct manufacturing years
                using var reader = await cmd.ExecuteReaderAsync(); // Execute query and read results asynchronously

                // Loop through the results
                while (await reader.ReadAsync())
                {
                    years.Add(reader.GetInt32(0)); // Add manufacturing year to the list
                }

                return years; // Return the list of manufacturing years
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting manufacturing years"); // Log the error
                throw; // Rethrow the exception
            }
        }

        // Asynchronous method to check if a specific car is available for given dates
        public async Task<bool> IsCarAvailableAsync(int carId, DateTime pickupDate, DateTime returnDate)
        {
            try
            {
                using var conn = _db.GetConnection(); // Get a new database connection
                await conn.OpenAsync(); // Open the connection asynchronously

                // SQL query to check car availability
                var query = @"
                    SELECT 1
                    FROM cars c
                    WHERE c.car_id = @CarId 
                    AND c.status = 'active'
                    AND NOT EXISTS (
                        SELECT 1 
                        FROM rentals r
                        WHERE r.car_id = c.car_id
                        AND r.status NOT IN ('cancelled', 'completed')
                        AND (
                            (@PickupDate BETWEEN r.pickup_date AND r.return_date)
                            OR (@ReturnDate BETWEEN r.pickup_date AND r.return_date)
                            OR (r.pickup_date BETWEEN @PickupDate AND @ReturnDate)
                        )
                    )";

                using var cmd = new SqlCommand(query, conn); // Create SQL command
                cmd.Parameters.AddWithValue("@CarId", carId); // Add car ID parameter
                cmd.Parameters.AddWithValue("@PickupDate", pickupDate); // Add pickup date parameter
                cmd.Parameters.AddWithValue("@ReturnDate", returnDate); // Add return date parameter

                var result = await cmd.ExecuteScalarAsync(); // Execute query and get the result
                return result != null; // Return true if the car is available, false otherwise
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking car availability"); // Log the error
                throw; // Rethrow the exception
            }
        }

        // Asynchronous method to get all available cars
        public async Task<IEnumerable<CarResponseDto>> GetAllCarsAsync()
        {
            try
            {
                using var conn = _db.GetConnection(); // Get a new database connection
                await conn.OpenAsync(); // Open the connection asynchronously

                // SQL query to select all active cars
                var query = "SELECT * FROM cars WHERE status = 'active'";
                using var cmd = new SqlCommand(query, conn); // Create SQL command

                var cars = new List<CarResponseDto>(); // List to hold car response DTOs
                using var reader = await cmd.ExecuteReaderAsync(); // Execute query and read results asynchronously

                // Loop through the results
                while (await reader.ReadAsync())
                {
                    // Add each active car to the list
                    cars.Add(new CarResponseDto
                    {
                        CarId = reader.GetInt32(reader.GetOrdinal("car_id")),
                        CarType = reader.GetString(reader.GetOrdinal("car_type")),
                        CarName = reader.GetString(reader.GetOrdinal("car_name")),
                        Transmission = reader.GetString(reader.GetOrdinal("transmission")),
                        PassengerCount = reader.GetInt32(reader.GetOrdinal("passenger_count")),
                        ManufacturingYear = reader.GetInt32(reader.GetOrdinal("manufacturing_year")),
                        DailyRate = reader.GetDecimal(reader.GetOrdinal("daily_rate")),
                        ImageUrl = reader.GetString(reader.GetOrdinal("image_url"))
                    });
                }

                return cars; // Return the list of all available cars
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all cars"); // Log the error
                throw; // Rethrow the exception
            }
        }
    }
}
