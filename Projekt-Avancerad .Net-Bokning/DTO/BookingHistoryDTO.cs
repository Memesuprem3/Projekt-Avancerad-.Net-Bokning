namespace Projekt_Avancerad_.Net_Bokning.DTO
{
    public class BookingHistoryDTO
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangeType { get; set; }
        public string ChangedBy { get; set; }
    }
}
