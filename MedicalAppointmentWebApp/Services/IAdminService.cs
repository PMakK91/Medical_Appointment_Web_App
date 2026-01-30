using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<DoctorRegistrationForm>> GetPendingRegistrations();
    }
}
