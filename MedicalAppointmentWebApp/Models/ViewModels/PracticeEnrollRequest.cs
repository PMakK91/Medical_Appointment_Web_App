namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class PracticeEnrollRequest
    {
        public string? City { get; set; }
        public string? Street { get; set; }
        public int StreetNum { get; set; }
        public string? PostalCode { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public int AppointmentDuration { get; set; }
    }
}
