using System.ComponentModel.DataAnnotations;

namespace MedicalAppointmentWebApp.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        public string Surname { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Amka { get; set; }
        
        public int Gender { get; set; }

        [Required]
        public int Phone { get; set; }
        
        public DateOnly BirthDate { get; set; }
        
        private string password;
    }
}
