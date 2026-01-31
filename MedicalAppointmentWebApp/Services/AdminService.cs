using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Services
{
    public class AdminService(AppDbContext _context, IConfiguration _configuration): IAdminService
    {
        public async Task<IEnumerable<Doctor>> GetPendingRegistrations()
        {
            return _context.Doctors.Where(
                d => d.RegistrationStatus == Data.Enums.RegistrationFormStatus.Pending).AsNoTracking().ToList();
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
    }
}
