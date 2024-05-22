namespace Projekt_Avancerad_.Net_Bokning.DTO
{
    public class AppointmentCreateDTO
    {
        public string AppointDiscription { get; set; }
        public DateTime PlacedApp { get; set; }
        public int CustomerId { get; set; }
        public int CompanyId { get; set; }

    }
}
