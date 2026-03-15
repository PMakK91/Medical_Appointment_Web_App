using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Services
{
    public interface IAppointmentService
    {
        Task<Appointment?> GetAppointmentByIdAsync(Guid id);
        Task<AppointmentRequestViewModel?> GetPracticeById(Guid practiceId, AppointmentRequestViewModel request);
        Task<Appointment?> CreateAppointmentAsync(AppointmentRequestViewModel request);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId);
        Task<IEnumerable<AppointmentWithPatientInfoViewModel>> GetAppointmentsByDayAndPracticeIdAsync(DateTime date, Guid practiceId);
        Task<IEnumerable<Appointment>> GetAppointmentsByMonthAndPracticeIdAsync(DateTime date, Guid practiceId);
        Task<Appointment?> CancelAppointmentAsync(Guid appointmentId);
        Task<Appointment?> NoShowAppointmentAsync(Guid id);
        Task<Appointment?> CompleteAppointmentAsync(Guid id);
    }
}
