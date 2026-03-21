using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IAppointmentRepository : IDisposable
    {
        Task<Appointment?> GetAppointmentByIdAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId);
        Task<Appointment?> GetAppointmentByTimeAsync(DateTime time, Guid practiceId);
        Task<IEnumerable<Appointment>> GetAllByPracticeAndTimeFrameAsync(DateTime startTime, DateTime endTime, Guid practiceId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDayAndPracticeIdAsync(DateTime day, Guid practiceId);
        Task<IEnumerable<AppointmentWithPatientInfoViewModel>> GetAppointmentsInfoByDayAndPracticeIdAsync(DateTime day, Guid practiceId);
        Task<IEnumerable<Appointment>> GetAppointmentsInfoByMonthAndPracticeIdAsync(DateTime date, Guid practiceId);
        Task<List<Appointment>> GetAllByPracticeAfterTodayAsync(Guid practiceId);
        Task<IEnumerable<Appointment>> GetAppointmentsAffectedByChangeAsync(EditPracticeViewModel editPracticeViewModel);

        void InsertAppointment(Appointment appointment);
        void UpdateAppointmentStatus(Guid appointmentId, AppointmentStatus status);

        void Save();
        Task SaveAsync();
        Task<DateTime?> GetFirstAvailableSlotForPracticeAsync(Guid practiceId);
        Task<(DateTime? Slot, Practice? Practice)> GetFirstAvailableSlotForDoctorAsync(Guid doctorId, double? lon, double? lat);
    }
}
