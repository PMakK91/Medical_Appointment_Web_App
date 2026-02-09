using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IDoctorRepository: IDisposable
    {
        Task<IEnumerable<Doctor>> GetDoctorsAsync();
        Task<IEnumerable<Doctor>> GetPendingDoctorsAsync();
        Task<IEnumerable<Doctor>> GetAcceptedDoctorsAsync();
        Task<Doctor?> GetDoctorByIDAsync(Guid doctorId);

        Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchActiveAsync(string term, int page, int pageSize);
        Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchPendingAsync(string term, int page, int pageSize);

    }
}
