using MedicalAppointmentWebApp.Data.Enums;

namespace MedicalAppointmentWebApp.Models.Security
{
    public class RegisterRequest
    {
        public required string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Amka { get; set; }
        public int Gender { get; set; }
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public required string Password { get; set; }

        public string? ConfirmPassword { get; set; }
        public Specialty? Specialty { get; set; }
        public string? LicenseNumber { get; set; }
    }
}
