namespace MedicalAppointmentWebApp.Models.Security
{
    public class RegisterRequest
    {
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Amka { get; set; } = "";
        public int Gender { get; set; } = 0;
        public string Phone { get; set; } = "";
        public string BirthDate { get; set; } = "";
        public string Password { get; set; } = "";
        public int Role { get; set; } = 0;
    }
}
