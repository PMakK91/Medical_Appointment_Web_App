using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

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
            return await context.Doctors.Include(d => d.Practices).FirstOrDefaultAsync(d => d.Id == doctorId);
        }

        public async Task<Doctor?> GetDoctorByEmailAsync(string email)
        {
            return await context.Doctors.FirstOrDefaultAsync(d => d.Email!.Equals(email));
        }

        public async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchActiveAsync(string term, double? lon, double? lat, int page, int pageSize)
        {
            return await SearchAsync(term, lon, lat, page, pageSize, RegistrationFormStatus.Accepted);
        }

        public async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchPendingAsync(string term, int page, int pageSize)
        {
            return await SearchAsync(term, null, null, page, pageSize, RegistrationFormStatus.Pending);
        }

        private async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchAsync(string term, double? lon, double? lat, int page, int pageSize, RegistrationFormStatus status)
        {
            IQueryable<Doctor> query = context.Doctors
                .Where(d => d.RegistrationStatus == status);

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(d =>
                    (d.FirstName != null && d.FirstName.Contains(term)) ||
                    (d.LastName != null && d.LastName.Contains(term)) ||
                    (d.Email != null && d.Email.Contains(term)) ||
                    (d.Specialty.ToString().Contains(term))
                );
            }

            if (lon != null && lat != null)
            {
                double maxDistanceKm = 10.0;
                double R = 6371.0; // radius of earth in km

                var doctorIds = await context.Practices
                    .Where(p =>
                        R * 2 * Math.Asin(
                            Math.Sqrt(
                                Math.Pow(Math.Sin(((p.Lat - lat.Value) * Math.PI / 180) / 2), 2) +
                                Math.Cos(lat.Value * Math.PI / 180) *
                                Math.Cos(p.Lat * Math.PI / 180) *
                                Math.Pow(Math.Sin(((p.Lon - lon.Value) * Math.PI / 180) / 2), 2)
                            )
                        ) <= maxDistanceKm
                    )
                    .Select(p => p.DoctorId)
                    .Distinct()
                    .ToListAsync();

                query = query.Where(d => doctorIds.Contains(d.Id));
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
