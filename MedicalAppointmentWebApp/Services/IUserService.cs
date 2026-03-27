using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(Guid id);

        Task<string?> UpdatePatientInfo(Guid patientId, EditPatientProfileViewModel request);

    }
}
