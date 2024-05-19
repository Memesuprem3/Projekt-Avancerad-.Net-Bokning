namespace Projekt_Avancerad_.Net_Bokning.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public string AppointmentDescription { get; set; }
        public DateTime PlacedApp { get; set; }
        public int CustomerId { get; set; }
        public int CompanyId { get; set; }
    }
}
