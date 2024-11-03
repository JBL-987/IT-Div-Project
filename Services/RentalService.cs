using RentalMobil.Models.DTOs;
using RentalMobil.Services;
using RentalMobil.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging; // Ensure to include the necessary namespace for ILogger
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalMobil.Services
{
    public class RentalService : IRentalService
    {
        private readonly DatabaseConnection _db; // Fixed the declaration of the _db field
        private readonly ICarService _carService;
        private readonly ILogger<RentalService> _logger;

        public RentalService(DatabaseConnection db, ICarService carService, ILogger<RentalService> logger)
        {
            _db = db; // Properly assign the db connection
            _carService = carService;
            _logger = logger;
        }

        public async Task<RentalResponseDto> CreateRentalAsync(RentalRequestDto request)
        {
            try
            {
                // Validate dates
                if (request.PickupDate < DateTime.Today)
                    throw new ArgumentException("Pickup date cannot be in the past");

                if (request.ReturnDate < request.PickupDate)
                    throw new ArgumentException("Return date must be after pickup date");

                // Check car availability
                var isAvailable = await _carService.IsCarAvailableAsync(
                    request.CarId,
                    request.PickupDate,
                    request.ReturnDate);

                if (!isAvailable)
                    throw new InvalidOperationException("Car is not available for the selected dates");

                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                using var transaction = conn.BeginTransaction();
                try
                {
                    // Get car details for price calculation
                    var car = await _carService.GetCarByIdAsync(request.CarId);
                    if (car == null)
                        throw new InvalidOperationException("Car not found");

                    // Calculate rental duration and total price
                    var duration = (request.ReturnDate - request.PickupDate).Days + 1;
                    var totalPrice = duration * car.DailyRate;

                    // Get user ID from email
                    var userId = await GetUserIdByEmailAsync(conn, transaction, request.RenterEmail);

                    // Create rental record
                    var insertQuery = @"
                        INSERT INTO rentals (
                            user_id, car_id, pickup_date, return_date, 
                            total_price, status, created_at
                        )
                        OUTPUT INSERTED.rental_id
                        VALUES (
                            @UserId, @CarId, @PickupDate, @ReturnDate,
                            @TotalPrice, 'pending', GETDATE()
                        )";

                    using var cmd = new SqlCommand(insertQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CarId", request.CarId);
                    cmd.Parameters.AddWithValue("@PickupDate", request.PickupDate);
                    cmd.Parameters.AddWithValue("@ReturnDate", request.ReturnDate);
                    cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);

                    var rentalId = (int)await cmd.ExecuteScalarAsync(); // Capture the rental ID

                    // Commit transaction
                    await transaction.CommitAsync();

                    // Return rental details
                    return new RentalResponseDto
                    {
                        RentalId = rentalId,
                        CarDetails = car,
                        PickupDate = request.PickupDate,
                        ReturnDate = request.ReturnDate,
                        TotalPrice = totalPrice,
                        Status = "pending"
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating rental");
                throw;
            }
        }

        public async Task<IEnumerable<RentalResponseDto>> GetUserRentalsAsync(string userEmail)
        {
            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT r.*, c.*
                    FROM rentals r
                    JOIN cars c ON r.car_id = c.car_id
                    JOIN users u ON r.user_id = u.user_id
                    WHERE u.email = @UserEmail
                    ORDER BY r.pickup_date DESC";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserEmail", userEmail);

                var rentals = new List<RentalResponseDto>();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    rentals.Add(new RentalResponseDto
                    {
                        RentalId = reader.GetInt32(reader.GetOrdinal("rental_id")),
                        CarDetails = new CarResponseDto
                        {
                            CarId = reader.GetInt32(reader.GetOrdinal("car_id")),
                            CarType = reader.GetString(reader.GetOrdinal("car_type")),
                            CarName = reader.GetString(reader.GetOrdinal("car_name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("image_url")),
                            Transmission = reader.GetString(reader.GetOrdinal("transmission"))
                        },
                        PickupDate = reader.GetDateTime(reader.GetOrdinal("pickup_date")),
                        ReturnDate = reader.GetDateTime(reader.GetOrdinal("return_date")),
                        TotalPrice = reader.GetDecimal(reader.GetOrdinal("total_price")),
                        Status = reader.GetString(reader.GetOrdinal("status"))
                    });
                }

                return rentals;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user rentals");
                throw;
            }
        }

        public async Task<bool> CancelRentalAsync(int rentalId, string userEmail)
        {
            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                using var transaction = conn.BeginTransaction();
                try
                {
                    // Verify rental belongs to user
                    var verifyQuery = @"
                        SELECT 1 FROM rentals r
                        JOIN users u ON r.user_id = u.user_id
                        WHERE r.rental_id = @RentalId 
                        AND u.email = @UserEmail
                        AND r.status = 'pending'
                        AND r.pickup_date > GETDATE()";

                    using var verifyCmd = new SqlCommand(verifyQuery, conn, transaction);
                    verifyCmd.Parameters.AddWithValue("@RentalId", rentalId);
                    verifyCmd.Parameters.AddWithValue("@UserEmail", userEmail);

                    var userExists = await verifyCmd.ExecuteScalarAsync();
                    if (userExists == null)
                    {
                        throw new InvalidOperationException("Rental not found or does not belong to the user.");
                    }

                    // Update rental status to canceled
                    var updateQuery = @"
                        UPDATE rentals
                        SET status = 'canceled'
                        WHERE rental_id = @RentalId";

                    using var updateCmd = new SqlCommand(updateQuery, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@RentalId", rentalId);
                    await updateCmd.ExecuteNonQueryAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    return true; // Rental successfully canceled
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling rental");
                throw;
            }
        }

        private async Task<int> GetUserIdByEmailAsync(SqlConnection conn, SqlTransaction transaction, string email)
        {
            var query = "SELECT user_id FROM users WHERE email = @Email";

            using var cmd = new SqlCommand(query, conn, transaction);
            cmd.Parameters.AddWithValue("@Email", email);

            var userId = await cmd.ExecuteScalarAsync();
            if (userId == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return (int)userId; // Return the user ID
        }
    }
}
