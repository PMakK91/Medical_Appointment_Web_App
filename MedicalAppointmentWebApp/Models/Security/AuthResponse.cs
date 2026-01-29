using System.Text.Json.Serialization;

namespace MedicalAppointmentWebApp.Models.Security
{
    public class AuthResponse
    {
        public required string AccessToken { get; set; } = "";
        public required string RefreshToken { get; set; } = "";
    }
}
