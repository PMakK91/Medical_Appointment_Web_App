namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class AppointmentRequestViewModel
    {
        public Guid PracticeId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime Time { get; set; }

        public Practice? Practice { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
