using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<Doctor>> GetPendingRegistrations();
        Task<Doctor?> AcceptRegistrationAsync(Guid id);
        Task<Doctor?> RejectRegistrationAsync(Guid id);
        Task<Doctor?> ViewRegistrationAsync(Guid id);
    }
}
