using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IAppointmentRepository : IDisposable
    {
        void InsertAppointment(Appointment appointment);

        void Save();
        Task SaveAsync();
    }
}
