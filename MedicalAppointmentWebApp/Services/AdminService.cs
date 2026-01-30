using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Services
{
    public class AdminService(AppDbContext _context, IConfiguration _configuration): IAdminService
    {
        public async Task<IEnumerable<DoctorRegistrationForm>> GetPendingRegistrations()
        {
            return _context.DoctorRegistrationForms.Where(
                d => d.Status == Data.Enums.RegistrationFormStatus.Pending).AsNoTracking().ToList();
        }
    }
}
