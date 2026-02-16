namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class TimeSlotViewModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsBooked { get; set; }
    }
}
