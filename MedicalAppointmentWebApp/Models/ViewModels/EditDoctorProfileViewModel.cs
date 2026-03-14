using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class EditDoctorProfileViewModel
    {
        // Display info
        public Doctor? Doctor { get; set; }

        // Change info
        public IFormFile? Image { get; set; }
        public string? Bio { get; set; }
    }
}
