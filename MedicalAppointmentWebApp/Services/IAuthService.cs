using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;
using MedicalAppointmentWebApp.Models.ViewModels;

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

        Task<string?> CreateResetPasswordTokenAsync(string email);
        Task<PasswordResetToken?> ResetPassword(NewPasswordRequestViewModel model);

        string? AmkaCheck(string amka);
        string? PhoneCheck(string phone);
    }
}
