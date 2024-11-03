using RentalMobil.Models.DTOs; // Importing the Data Transfer Objects (DTOs) used in rental operations

namespace RentalMobil.Services // Declaring the namespace for the rental service
{
    // Defining the interface for rental-related operations
    public interface IRentalService
    {
        /// <summary>
        /// Creates a new rental based on the provided rental request DTO.
        /// </summary>
        /// <param name="rentalRequest">The rental request containing necessary details.</param>
        /// <returns>A Task that represents the asynchronous operation, containing the created rental response DTO.</returns>
        Task<RentalResponseDto> CreateRentalAsync(RentalRequestDto rentalRequest);

        /// <summary>
        /// Retrieves all rentals associated with a specific user identified by their email.
        /// </summary>
        /// <param name="userEmail">The email of the user whose rentals are to be retrieved.</param>
        /// <returns>A Task that represents the asynchronous operation, containing a collection of rental response DTOs.</returns>
        Task<IEnumerable<RentalResponseDto>> GetUserRentalsAsync(string userEmail);

        /// <summary>
        /// Cancels a rental based on the provided rental ID, ensuring the user has the right to cancel it.
        /// </summary>
        /// <param name="rentalId">The ID of the rental to be canceled.</param>
        /// <param name="userEmail">The email of the user requesting the cancellation.</param>
        /// <returns>A Task that represents the asynchronous operation, returning true if the cancellation was successful, otherwise false.</returns>
        Task<bool> CancelRentalAsync(int rentalId, string userEmail);
    }
}
