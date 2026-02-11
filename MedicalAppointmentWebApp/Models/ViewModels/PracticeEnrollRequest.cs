namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class PracticeEnrollRequest
    {
        public Guid DoctorId { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int StreetNum { get; set; }
        public string? PostalCode { get; set; }
    }
}
