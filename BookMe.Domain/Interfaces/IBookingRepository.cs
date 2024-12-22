using BookMe.Domain.Entities;

public interface IBookingRepository
{
    Task Create(Booking booking);
    Task<IEnumerable<Booking>> GetBookingsByUserId(string userId);
    Task<IEnumerable<Booking>> GetBookingsByEmployeeId(int employeeId);
    Task<Booking> GetBookingByIdAsync(int id);
    Task UpdateBookingAsync(Booking booking);
    Task DeleteBookingAsync(int id);
    Task<bool> HasOpinion(int bookingId);
    Task<Opinion> GetOpinionByBookingIdAsync(int bookingId);

    Task<List<Booking>> GetBookingsByEmployeeIdAsync(int employeeId);

    Task<IEnumerable<Booking>> GetBookingsByServiceEncodedName(string encodedName, string searchTerm = null);
}
