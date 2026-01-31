using MedicalAppointmentWebApp.Data.Enums;

namespace MedicalAppointmentWebApp.Models.Security
{
    public class DoctorRegisterFormRequest
    {
        public Guid Id { get; set; }
        public required string Email { get; set; } = "";
        public required string Password { get; set; } = "";
        public required string ConfirmPassword { get; set; } = "";
        public required string Amka { get; set; } = "";
        public required Specialty Specialty { get; set; }
        public string? Name { get; set; } = "";
        public string? LastName { get; set; } = "";
        public string? Phone { get; set; } = "";
        public int Gender { get; set; } = -1;
        public DateOnly? BirthDate { get; set; } = DateOnly.MinValue;
    }
}
