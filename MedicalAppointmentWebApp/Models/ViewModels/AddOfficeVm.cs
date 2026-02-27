namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class AddOfficeVm
    {
        public required string Name { get; set; }
        public required int Duration { get; set; }
        public required int PlaceId { get; set; }
        public required int Phone { get; set; }

        public int MondayStart { get; set; }
        public int MondayEnd { get; set; }
        public int TuesdayStart { get; set; }
        public int TuesdayEnd { get; set; }
        public int WednesdayStart { get; set; }
        public int WednesdayEnd { get; set; }
        public int ThursdayStart { get; set; }
        public int ThursdayEnd { get; set; }
        public int FridayStart { get; set; }
        public int FridayEnd { get; set; }
        public int SaturdayStart { get; set; }
        public int SaturdayEnd { get; set; }
        public int SundayStart { get; set; }
        public int SundayEnd { get; set; }
    }
}
