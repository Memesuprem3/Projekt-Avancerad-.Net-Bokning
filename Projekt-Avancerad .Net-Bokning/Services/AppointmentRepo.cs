using Microsoft.EntityFrameworkCore;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Models;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class AppointmentRepo : IAppointment
    {
        private AppDbContext _context;

        public AppointmentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            await AddBookingHistoryAsync(appointment.id, "Created");
            return appointment;
        }

       
        public async Task<Appointment> DeleteAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            await AddBookingHistoryAsync(appointment.id, "Deleted");
            return appointment;
        }

        public async Task<Appointment> GetAppointmentAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentDayAsync(DateTime date)
        {
            return await _context.Appointments
                                 .Where(a => a.PlacedApp.Date == date.Date).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentMonthAsync(int year, int month)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentWeekAsync(int year, int week)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentYearAsync(int year)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BookingHistory>> GetBookingHistoryAsync(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        private async Task AddBookingHistoryAsync(int id, string v)
        {
            throw new NotImplementedException();
        }
    }
}
