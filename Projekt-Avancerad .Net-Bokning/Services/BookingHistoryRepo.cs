using Microsoft.EntityFrameworkCore;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class BookingHistoryRepo : IBookingHistory
    {
        private readonly AppDbContext _context;

        public BookingHistoryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBookingHistoryAsync(BookingHistory bookingHistory)
        {
            _context.BookingHistories.Add(bookingHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingHistory>> GetBookingHistoriesByAppointmentIdAsync(int appointmentId)
        {
            return await _context.BookingHistories
                .Where(bh => bh.AppointmentId == appointmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingHistory>> GetAllBookingHistoriesAsync()
        {
            return await _context.BookingHistories.ToListAsync();
        }
    }
}