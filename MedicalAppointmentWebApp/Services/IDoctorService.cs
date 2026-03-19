using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Services
{
    public interface IDoctorService
    {
        Task<DoctorSearchViewModel> SearchAllActiveDoctorsAsync(
            string term,
            double? lon,
            double? lat,
            double? userLon,
            double? userLat,
            int page,
            int pageSize,
            string? sortBy); 
        
        Task<Doctor?> GetDoctorById(Guid id);

        Task<string?> UpdateDoctorInfo(Guid doctorId, EditDoctorProfileViewModel request);

        Task<List<Practice>> GetPracticesAsync(Guid doctorId);

    }
}
