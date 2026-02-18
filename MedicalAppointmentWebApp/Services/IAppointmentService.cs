using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentRequestViewModel?> GetPracticeById(Guid practiceId, AppointmentRequestViewModel request);
        Task<Appointment?> CreateAppointmentAsync(AppointmentRequestViewModel request);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(Guid patientId);
    }
}
