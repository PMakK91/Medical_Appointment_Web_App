using MedicalAppointmentWebApp.Data;
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
    }
}
