using MedicalAppointmentWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentWebApp.Models
{
    public class Doctor: User
    {
        public Doctor()
        {
            Role = UserRole.Doctor;
        }

        [Column("image")]
        public string? ImageUrl { get; set; }

        [Column("bio")]
        [StringLength(500)]
        public string? Bio { get; set; }

        [Column("specialty")]
        public Specialty Specialty { get; set; }

        [Column("registration_status")]
        public RegistrationFormStatus RegistrationStatus { get; set; }
    }
}
