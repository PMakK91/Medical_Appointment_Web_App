using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Services
{
    public class AdminService(IDoctorRepository doctorRepository, IPracticeRepository practiceRepository) : IAdminService
    {
        public async Task<DoctorSearchViewModel> GetPendingRegistrations(string term, int page = 1, int pageSize = 10)
        {
            (IEnumerable<Doctor>, int) doctorsAndCount = await doctorRepository.SearchPendingAsync(term, page, pageSize);

            return new DoctorSearchViewModel
            {
                Doctors = doctorsAndCount.Item1,
                Term = term,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = doctorsAndCount.Item2
            };
        }

        public async Task<Doctor?> AcceptRegistrationAsync(Guid id)
        {
            return await ReviewRegistrationAsync(id, RegistrationFormStatus.Accepted);
        }

        public async Task<Doctor?> RejectRegistrationAsync(Guid id)
        {
            return await ReviewRegistrationAsync(id, RegistrationFormStatus.Rejected);
        }

        private async Task<Doctor?> ReviewRegistrationAsync(Guid id, RegistrationFormStatus status)
        {
            Doctor? doctor = await doctorRepository.GetDoctorByIDAsync(id);

            if (doctor is null)
            {
                return null;
            }

            doctor.RegistrationStatus = status;
            await doctorRepository.SaveAsync();

            return doctor;
        }

        public async Task<Doctor?> ViewRegistrationAsync(Guid id)
        {
            Doctor? doctor = await doctorRepository.GetDoctorByIDAsync(id);

            if (doctor is null)
            {
                return null;
            }

            return doctor;
        }

        public async Task<PracticeSearchViewModel> SearchPendingPractices(string term, int page = 1, int pageSize = 10)
        {
            (IEnumerable<Practice>, int) practicesAndCount = await practiceRepository.SearchPendingAsync(term, page, pageSize);

            return new PracticeSearchViewModel
            {
                Practices = practicesAndCount.Item1,
                Term = term,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = practicesAndCount.Item2
            };
        }
    }
}
