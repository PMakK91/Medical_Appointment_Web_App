using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IPracticeRepository : IDisposable
    {
        Task<Practice?> GetPracticeByIdAsync(Guid id);
        Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchPendingAsync(string term, int page = 1, int pageSize = 10);
        Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchAllWithDoctorIdAsync(Guid doctorId, string term, int page = 1, int pageSize = 10);
        void InsertPractice(Practice practice);

        Task<List<Practice>> GetPracticesByDoctorIdAsync(Guid doctorId);

        void Save();
        Task SaveAsync();
    }
}
