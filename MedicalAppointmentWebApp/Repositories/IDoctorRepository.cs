using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IDoctorRepository: IDisposable
    {
        Task<IEnumerable<Doctor>> GetDoctorsAsync();
        Task<IEnumerable<Doctor>> GetPendingDoctorsAsync();
        Task<IEnumerable<Doctor>> GetAcceptedDoctorsAsync();
        void InsertDoctor(Doctor doctor);
        Task<Doctor?> GetDoctorByIDAsync(Guid doctorId);
        Task<Doctor?> GetDoctorByEmailAsync(string email);

        Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchActiveAsync(string term, double? lon, double? lat, int page, int pageSize, string? sortBy);
        Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchPendingAsync(string term, int page, int pageSize);

        void Save();
        Task SaveAsync();

    }
}
