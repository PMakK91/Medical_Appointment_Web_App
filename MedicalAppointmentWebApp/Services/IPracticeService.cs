using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Services
{
    public interface IPracticeService
    {
        Task<Practice?> EnrollPracticeAsync(Guid doctorId, PracticeEnrollRequest practiceEnrollRequest);
        Task<Practice?> GetPracticeByIdAsync(Guid id);
        Task<WeekScheduleViewModel?> GetPracticeByIdAsync(Guid id, DateTime? weekStart);
        Task<PracticeSearchViewModel> GetPracticesWithDoctorIdAsync(Guid doctorId, string term, int page, int pageSize);

        Task<AvailableSlotsViewModel?> GetPracticeAvailableSlotsAsync(Guid id);





    }
}
