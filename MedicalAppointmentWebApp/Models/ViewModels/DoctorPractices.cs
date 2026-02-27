namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class DoctorPractices
    {
        public Doctor Doctor { get; set; } = null!;
        public List<Practice> Practices { get; set; } = new();
    }
}
