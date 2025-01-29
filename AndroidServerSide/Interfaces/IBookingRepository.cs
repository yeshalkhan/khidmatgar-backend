using AndroidServerSide.Models;

namespace AndroidServerSide.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByUserEmailAsync(string userEmail);
        Task<IEnumerable<Booking>> GetBookingsByServiceProviderIdAsync(int serviceProviderId);
        Task<int?> AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task<Booking?> DeleteBookingAsync(int id);
    }
}
