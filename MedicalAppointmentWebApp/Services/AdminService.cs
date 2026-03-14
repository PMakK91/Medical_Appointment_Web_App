using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Services
{
    public class AdminService(IDoctorRepository doctorRepository, IPracticeRepository practiceRepository, IEmailService emailService) : IAdminService
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

        public async Task<Doctor?> RejectRegistrationAsync(Guid id, string reason)
        {
            return await ReviewRegistrationAsync(id, RegistrationFormStatus.Rejected, reason);
        }

        private async Task<Doctor?> ReviewRegistrationAsync(Guid id, RegistrationFormStatus status, string? reason = null)
        {
            Doctor? doctor = await doctorRepository.GetDoctorByIDAsync(id);

            if (doctor is null)
            {
                return null;
            }

            if (status == RegistrationFormStatus.Accepted)
            {
                await emailService.SendAcceptDoctorRegistrationEmail(doctor.Email);
            }
            else if (status == RegistrationFormStatus.Rejected)
            {
                await emailService.SendRejectDoctorRegistrationEmail(doctor.Email, reason);
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

        public async Task<Practice?> AcceptPracticeAsync(Guid id)
        {
            return await ReviewPracticeAsync(id, RegistrationFormStatus.Accepted);
        }

        public async Task<Practice?> RejectPracticeAsync(Guid id)
        {
            return await ReviewPracticeAsync(id, RegistrationFormStatus.Rejected);
        }

        private async Task<Practice?> ReviewPracticeAsync(Guid id, RegistrationFormStatus status)
        {
            Practice? practice = await practiceRepository.GetPracticeByIdAsync(id);
            if (practice is null)
            {
                return null;
            }

            practice.RegistrationStatus = status;
            await practiceRepository.SaveAsync();

            return practice;
        }

        public async Task<Practice?> ViewPracticeByIdAsync(Guid id)
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
