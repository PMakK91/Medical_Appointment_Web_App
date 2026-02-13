using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IPracticeRepository : IDisposable
    {
        Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchPendingAsync(string term, int page = 1, int pageSize = 10);
        Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchAllWithDoctorIdAsync(Guid doctorId, string term, int page = 1, int pageSize = 10);
        void InsertPractice(Practice practice);

        void Save();
        Task SaveAsync();
    }
}
