using RentalMobil.Data;
using RentalMobil.Models;
using Microsoft.Data.SqlClient;

namespace RentalMobil.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection()
        {
            // Ganti dengan connection string sesuai dengan konfigurasi SQL Server Anda
            _connectionString = @"Server=LAPTOP-LOI;Database=db_rentcar;Trusted_Connection=True;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

public class CarRepository
{
    private readonly DatabaseConnection _db;

    public CarRepository()
    {
        _db = new DatabaseConnection();
    }

    public List<Car> GetAllCars()
    {
        var cars = new List<Car>();

        using (var conn = _db.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM cars WHERE status = 'available'", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car
                            {
                                CarId = reader.GetInt32(reader.GetOrdinal("car_id")),
                                CarType = reader.GetString(reader.GetOrdinal("car_type")),
                                CarName = reader.GetString(reader.GetOrdinal("car_name")),
                                Transmission = reader.GetString(reader.GetOrdinal("transmission")),
                                PassengerCount = reader.GetInt32(reader.GetOrdinal("passenger_count")),
                                ManufacturingYear = reader.GetInt32(reader.GetOrdinal("manufacturing_year")),
                                DailyRate = reader.GetDecimal(reader.GetOrdinal("daily_rate")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("image_url")),
                                Status = reader.GetString(reader.GetOrdinal("status"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error appropriately
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        return cars;
    }

    // Contoh method untuk menyimpan rental baru
    public bool CreateRental(int userId, int carId, DateTime pickupDate, DateTime returnDate, decimal totalPrice)
    {
        using (var conn = _db.GetConnection())
        {
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    INSERT INTO rentals (user_id, car_id, pickup_date, return_date, total_price)
                    VALUES (@UserId, @CarId, @PickupDate, @ReturnDate, @TotalPrice)", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CarId", carId);
                    cmd.Parameters.AddWithValue("@PickupDate", pickupDate);
                    cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                    cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}