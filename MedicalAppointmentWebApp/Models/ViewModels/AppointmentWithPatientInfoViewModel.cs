using MedicalAppointmentWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class AppointmentWithPatientInfoViewModel
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Time { get; set; }
        public AppointmentStatus Status { get; set; }

        public Guid PracticeId { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
    }
}
