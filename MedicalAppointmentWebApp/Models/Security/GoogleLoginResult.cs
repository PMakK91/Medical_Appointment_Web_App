namespace MedicalAppointmentWebApp.Models.Security
{
    public class GoogleLoginResult
    {
        public bool Success { get; set; }

        public bool IsNewUser { get; set; }

        public string? ErrorMessage { get; set; }

        public AuthResponse? Tokens { get; set; }

        public string? Email { get; set; }
    }
}
