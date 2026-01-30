namespace MedicalAppointmentWebApp.Models.Security
{
    public class RegisterRequest
    {
        public required string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Amka { get; set; } = "";
        public int Gender { get; set; } = 0;
        public string Phone { get; set; } = "";
        public DateOnly? BirthDate { get; set; } = DateOnly.MinValue;
        public required string Password { get; set; } = "";

        public required string ConfirmPassword { get; set; } = "";
    }
}
