using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Services
{
    public class AdminService(AppDbContext _context): IAdminService
    {
        public async Task<DoctorSearchViewModel> GetPendingRegistrations(string term, int page = 1, int pageSize = 10)
        {
            var query = _context.Doctors.Where(
                d => d.RegistrationStatus == RegistrationFormStatus.Pending);

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(d =>
                (d.FirstName != null && d.FirstName.Contains(term)) ||
                (d.LastName != null && d.LastName.Contains(term)) ||
                (d.Email != null && d.Email.Contains(term)) ||
                (((Specialty)d.Specialty).ToString().Contains(term)));
            }

            int totalItems = await query.CountAsync();

            IEnumerable<Doctor> doctors = await query
                .AsNoTracking()
                .OrderBy(d => d.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new DoctorSearchViewModel
            {
                Doctors = doctors,
                Term = term,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
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
            Doctor? doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);

            if (doctor is null)
            {
                return null;
            }

            doctor.RegistrationStatus = status;
            await _context.SaveChangesAsync();

            return doctor;
        }

        public async Task<Doctor?> ViewRegistrationAsync(Guid id)
        {
            Doctor? doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);

            if (doctor is null)
            {
                return null;
            }

            return doctor;
        }
    }
}
