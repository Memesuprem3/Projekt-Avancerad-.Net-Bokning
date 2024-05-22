using Projekt_Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Services.Interface
{
    public interface IAppointment
    {
        Task<Appointment> AddAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment> GetAppointmentAsync(int id);
        Task<IEnumerable<Appointment>> GetAppointmentDayAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetAppointmentMonthAsync(int year, int month);
        Task<IEnumerable<Appointment>> GetAppointmentWeekAsync(int year, int week);
        Task<IEnumerable<BookingHistory>> GetBookingHistoryAsync(int appointmentId);
        Task UpdateAppointmentAsync(Appointment appointment);
    }
}