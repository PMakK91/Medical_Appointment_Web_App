using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllActiveDoctors();
        Task<Doctor?> GetDoctorById(Guid id);
    }
}
