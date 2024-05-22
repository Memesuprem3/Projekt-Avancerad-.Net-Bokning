using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services.Interface
{
    public interface IBookingHistory
    {
        Task AddBookingHistoryAsync(BookingHistory bookingHistory);
        Task<IEnumerable<BookingHistory>> GetBookingHistoriesByAppointmentIdAsync(int appointmentId);
        Task<IEnumerable<BookingHistory>> GetAllBookingHistoriesAsync();
    }
}
