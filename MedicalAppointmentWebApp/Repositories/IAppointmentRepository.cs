using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IAppointmentRepository : IDisposable
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId);
        Task<Appointment?> GetAppointmentByTimeAsync(DateTime time, Guid practiceId);
        Task<IEnumerable<Appointment>> GetAllByPracticeAndTimeFrameAsync(DateTime startTime, DateTime endTime, Guid practiceId);

        void InsertAppointment(Appointment appointment);

        void Save();
        Task SaveAsync();
    }
}
