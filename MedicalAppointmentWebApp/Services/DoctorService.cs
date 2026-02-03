using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Services
{
    public class DoctorService(AppDbContext _context) : IDoctorService
    {
        public async Task<DoctorSearchViewModel> SearchAllActiveDoctors(string term, int page = 1, int pageSize = 10)
        {
            var query = _context.Doctors.Where(
                d => d.RegistrationStatus == Data.Enums.RegistrationFormStatus.Accepted);

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

        public async Task<Doctor?> GetDoctorById(Guid id)
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
