using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Services
{
    public interface IAdminService
    {
        Task<DoctorSearchViewModel> GetPendingRegistrations(string term, int page, int pageSize);
        Task<Doctor?> AcceptRegistrationAsync(Guid id);
        Task<Doctor?> RejectRegistrationAsync(Guid id);
        Task<Doctor?> ViewRegistrationAsync(Guid id);
    }
}
