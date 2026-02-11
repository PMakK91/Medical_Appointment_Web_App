namespace MedicalAppointmentWebApp.Models
{
    public class Patient: User
    {
        public Patient()
        {
            Role = Data.Enums.UserRole.Patient;
        }

        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
