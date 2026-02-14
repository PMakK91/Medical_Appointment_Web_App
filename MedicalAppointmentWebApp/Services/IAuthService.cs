using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;

namespace MedicalAppointmentWebApp.Services
{
    public interface IAuthService
    {
        Task<Doctor?> DoctorRegisterAsync(RegisterRequest request);
        Task<Patient?> PatientRegisterAsync(RegisterRequest request);
        Task<Admin?> AdminRegisterAsync(RegisterRequest request);
        Task<User?> LogoutAsync(string refreshToken);
        Task<AuthResponse?> LoginAsync(AuthRequest request);
        Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request);
        string? AmkaCheck(string amka);
        string? PhoneCheck(string phone);
    }
}
