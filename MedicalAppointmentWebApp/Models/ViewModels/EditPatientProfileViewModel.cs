using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class EditPatientProfileViewModel
    {
        // Display info
        public User? User { get; set; }

        // Change info

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Amka { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? Gender { get; set; }



    }
}
