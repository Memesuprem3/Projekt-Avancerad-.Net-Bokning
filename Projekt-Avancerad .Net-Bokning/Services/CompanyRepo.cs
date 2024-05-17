using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class CompanyRepo : IAppointment
    {
        public Task<Appointment> AddAppointmentAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public Task<Appointment> DeleteAppointmentAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public Task<Appointment> GetAppointmentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAppointmentDayAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAppointmentMonthAsync(int year, int month)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAppointmentWeekAsync(int year, int week)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAppointmentYearAsync(int year)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookingHistory>> GetBookingHistoryAsync(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
