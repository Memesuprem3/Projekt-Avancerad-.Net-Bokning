using Microsoft.EntityFrameworkCore;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class AppointmentRepo : IAppointment
    {
        private readonly AppDbContext _context;
        private readonly IBookingHistory _bookingHistoryRepo;

        public AppointmentRepo(AppDbContext context, IBookingHistory bookingHistoryRepo)
        {
            _context = context;
            _bookingHistoryRepo = bookingHistoryRepo;
        }

        public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var bookingHistory = new BookingHistory
            {
                AppointmentId = appointment.id,
                ChangedAt = DateTime.UtcNow,
                ChangeType = "Added Appointment",
                ChangedBy = appointment.UserId.ToString()
            };
            await _bookingHistoryRepo.AddBookingHistoryAsync(bookingHistory);

            return appointment;
        }

        public async Task DeleteAppointmentAsync(Appointment appointment)
        {
            var bookingHistory = new BookingHistory
            {
                AppointmentId = appointment.id,
                ChangedAt = DateTime.UtcNow,
                ChangeType = "Deleted Appointment",
                ChangedBy = appointment.UserId.ToString()
            };

            await _bookingHistoryRepo.AddBookingHistoryAsync(bookingHistory);

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentDayAsync(DateTime date)
        {
            return await _context.Appointments.Where(a => a.PlacedApp.Date == date.Date).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentMonthAsync(int year, int month)
        {
            return await _context.Appointments.Where(a => a.PlacedApp.Year == year && a.PlacedApp.Month == month).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentWeekAsync(int year, int week)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var firstMonday = firstDayOfYear.AddDays(1 - (int)(firstDayOfYear.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDayOfYear.DayOfWeek));
            var firstDayOfWeek = firstMonday.AddDays((week - 1) * 7);
            var lastDayOfWeek = firstDayOfWeek.AddDays(7);

            return await _context.Appointments.Include(a => a.Customer).Include(a => a.Company)
                .Where(a => a.PlacedApp >= firstDayOfWeek && a.PlacedApp < lastDayOfWeek)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingHistory>> GetBookingHistoryAsync(int id)
        {
            return await _context.BookingHistories.Where(bh => bh.AppointmentId == id).ToListAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Entry(appointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var bookingHistory = new BookingHistory
            {
                AppointmentId = appointment.id,
                ChangedAt = DateTime.UtcNow,
                ChangeType = "Updated Appointment",
                ChangedBy = appointment.UserId.ToString()
            };
            await _bookingHistoryRepo.AddBookingHistoryAsync(bookingHistory);
        }
    }
}