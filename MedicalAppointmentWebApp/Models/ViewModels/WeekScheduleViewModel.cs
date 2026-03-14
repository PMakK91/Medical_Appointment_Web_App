namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class WeekScheduleViewModel
    {
        public Practice Practice { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<TimeSlotViewModel>[] Slots { get; set; } = new List<TimeSlotViewModel>[7];
    }
}