namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class DoctorRegistrationReviewVeiwModel
    {
        public Guid DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

        public string Reason { get; set; }
    }
}
