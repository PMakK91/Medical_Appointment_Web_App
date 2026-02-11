using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IPracticeRepository : IDisposable
    {
        void InsertPractice(Practice practice);

        void Save();
        Task SaveAsync();
    }
}
