using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public interface IAppointment
    {
        Task<Appointment> GetAppointmentAsync();
        Task<Appointment> AddAppointmentAsync(Appointment appointment);

        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<Appointment> DeleteAppointmentAsync(Appointment appointment);

    }
}
