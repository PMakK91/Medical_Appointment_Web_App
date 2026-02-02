using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Services
{
    public interface IDoctorService
    {
        Task<DoctorSearchViewModel> SearchAllActiveDoctors(string term, int page = 1, int pageSize = 10);
        Task<Doctor?> GetDoctorById(Guid id);
    }
}
