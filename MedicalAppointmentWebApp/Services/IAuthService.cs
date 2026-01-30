using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;

namespace MedicalAppointmentWebApp.Services
{
    public interface IAuthService
    {
        Task<DoctorRegistrationForm?> DoctorRegisterAsync(DoctorRegisterFormRequest request);
        Task<User?> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(AuthRequest request);
        Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
