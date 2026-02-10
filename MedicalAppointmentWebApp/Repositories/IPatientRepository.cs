using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IPatientRepository : IDisposable
    {
        void InsertPatient(Patient patient);

        void Save();
        Task SaveAsync();
    }
}
