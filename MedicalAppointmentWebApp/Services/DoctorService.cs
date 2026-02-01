using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Services
{
    public class DoctorService(AppDbContext _context) : IDoctorService
    {
        public async Task<IEnumerable<Doctor>> GetAllActiveDoctors()
        {
            return _context.Doctors.Where(
                d => d.RegistrationStatus == Data.Enums.RegistrationFormStatus.Accepted).AsNoTracking().ToList();
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
