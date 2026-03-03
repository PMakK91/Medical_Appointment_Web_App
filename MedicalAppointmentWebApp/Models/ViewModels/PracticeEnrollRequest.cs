namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class PracticeEnrollRequest
    {
        public  string Name { get; set; }
        public  int Duration { get; set; }
        public string PlaceId { get; set; }

        public double Lon { get; set; }

        public double Lat { get; set; }

        public string FullAddress { get; set; }

        public  int Phone { get; set; }

        public TimeSpan? MondayStart { get; set; }
        public TimeSpan? MondayEnd { get; set; }

        public TimeSpan? TuesdayStart { get; set; }
        public TimeSpan? TuesdayEnd { get; set; }

        public TimeSpan? WednesdayStart { get; set; }
        public TimeSpan? WednesdayEnd { get; set; }

        public TimeSpan? ThursdayStart { get; set; }
        public TimeSpan? ThursdayEnd { get; set; }

        public TimeSpan? FridayStart { get; set; }
        public TimeSpan? FridayEnd { get; set; }

        public TimeSpan? SaturdayStart { get; set; }
        public TimeSpan? SaturdayEnd { get; set; }

        public TimeSpan? SundayStart { get; set; }
        public TimeSpan? SundayEnd { get; set; }
    }
}
