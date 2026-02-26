namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class AddOfficeVm
    {
        public required string Name { get; set; }
        public required int Duration { get; set; }
        public required int PlaceId { get; set; }
        public List<DayOfWeekVm> WeeklyHours { get; set; } = new();
    }


    public class DayOfWeekVm
    {
        public DayOfWeek Day { get; set; }
        public bool Open { get; set; }
        public List<IntervalVm> Intervals { get; set; } = new();
    }

    public class IntervalVm
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
