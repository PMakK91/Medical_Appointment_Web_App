using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IAdminRepository : IDisposable
    {
        void InsertAdmin(Admin admin);

        void Save();
        Task SaveAsync();
    }
}
