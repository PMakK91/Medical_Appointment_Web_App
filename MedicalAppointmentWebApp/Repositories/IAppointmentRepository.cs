using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IAppointmentRepository : IDisposable
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId);

        void InsertAppointment(Appointment appointment);

        void Save();
        Task SaveAsync();
    }
}
