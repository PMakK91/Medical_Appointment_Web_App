using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;

namespace MedicalAppointmentWebApp.Services
{
    public class PracticeService(IDoctorRepository doctorRepository, IPracticeRepository practiceRepository) : IPracticeService
    {
        public async Task<Practice?> EnrollPracticeAsync(Guid doctorId, PracticeEnrollRequest practiceEnrollRequest)
        {
            Doctor? doctor = await doctorRepository.GetDoctorByIDAsync(doctorId);
            if (doctor == null)
            {
                return null;
            }

            Practice practice = new Practice();

            practice.DoctorId = doctor.Id;
            practice.City = practiceEnrollRequest.City;
            practice.Street = practiceEnrollRequest.Street;
            practice.StreetNum = practiceEnrollRequest.StreetNum;
            practice.PostalCode= practiceEnrollRequest.PostalCode;
            practice.ShiftStartTime = practiceEnrollRequest.ShiftStartTime;
            practice.ShiftEndTime = practiceEnrollRequest.ShiftEndTime;
            practice.AppointmentDuration = practiceEnrollRequest.AppointmentDuration;

            practice.RegistrationStatus = Data.Enums.RegistrationFormStatus.Pending;

            practiceRepository.InsertPractice(practice);
            await practiceRepository.SaveAsync();

            return practice;
        }

        public async Task<PracticeSearchViewModel> GetPracticesWithDoctorIdAsync(Guid doctorId, string term, int page, int pageSize)
        {
            (IEnumerable<Practice>, int) practicesAndCount = await practiceRepository.SearchAllWithDoctorIdAsync(doctorId, term, page, pageSize);

            return new PracticeSearchViewModel
            {
                Practices = practicesAndCount.Item1,
                Term = term,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = practicesAndCount.Item2
            };
        }

        public async Task<Practice?> GetPracticeByIdAsync(Guid id)
        {
            Practice? practice = await practiceRepository.GetPracticeByIdAsync(id);
            if (practice is null)
            {
                return null;
            }
            return practice;
        }
    }
}
