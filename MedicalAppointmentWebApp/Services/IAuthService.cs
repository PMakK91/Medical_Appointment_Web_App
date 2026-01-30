using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;

namespace MedicalAppointmentWebApp.Services
{
    public interface IAuthService
    {
        Task<Doctor?> DoctorRegisterAsync(DoctorRegisterFormRequest request);
        Task<Patient?> PatientRegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(AuthRequest request);
        Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
