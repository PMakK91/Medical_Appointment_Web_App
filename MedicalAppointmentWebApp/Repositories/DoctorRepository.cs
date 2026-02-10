using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedicalAppointmentWebApp.Repositories
{
    public class DoctorRepository(AppDbContext context) : IDoctorRepository, IDisposable
    {
        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            return await context.Doctors.ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetPendingDoctorsAsync()
        {
            return await context.Doctors.Where(d => d.RegistrationStatus.Equals(RegistrationFormStatus.Pending)).ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAcceptedDoctorsAsync()
        {
            return await context.Doctors.Where(d => d.RegistrationStatus.Equals(RegistrationFormStatus.Accepted)).ToListAsync();
        }

        public void InsertDoctor(Doctor doctor)
        {
            context.Doctors.Add(doctor);
        }

        public async Task<Doctor?> GetDoctorByIDAsync(Guid doctorId)
        {
            return await context.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId);
        }

        public async Task<Doctor?> GetDoctorByEmailAsync(string email)
        {
            return await context.Doctors.FirstOrDefaultAsync(d => d.Email!.Equals(email));
        }

        public async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchActiveAsync(string term, int page, int pageSize)
        {
            return await SearchAsync(term, page, pageSize, RegistrationFormStatus.Accepted);
        }

        public async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchPendingAsync(string term, int page, int pageSize)
        {
            return await SearchAsync(term, page, pageSize, RegistrationFormStatus.Pending);
        }

        private async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchAsync(string term, int page, int pageSize, RegistrationFormStatus status)
        {
            IQueryable<Doctor> query = context.Doctors.Where(d => d.RegistrationStatus == status);

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(d =>
                (d.FirstName != null && d.FirstName.Contains(term)) ||
                (d.LastName != null && d.LastName.Contains(term)) ||
                (d.Email != null && d.Email.Contains(term)) ||
                ((d.Specialty).ToString().Contains(term)));
            }

            int totalCount = await query.CountAsync();

            IEnumerable<Doctor> doctors = await query
                .AsNoTracking()
                .OrderBy(d => d.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (Doctors: doctors, TotalCount: totalCount);
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }



        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
