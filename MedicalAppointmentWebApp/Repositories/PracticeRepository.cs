using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MedicalAppointmentWebApp.Repositories
{
    public class PracticeRepository(AppDbContext context) : IPracticeRepository, IDisposable
    {
        public async Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchAllWithDoctorIdAsync(Guid doctorId, string term, int page = 1, int pageSize = 10)
        {
            IQueryable<Practice> query = context.Practices.Where(p => p.DoctorId == doctorId);

            return await SearchAsync(query, term, page, pageSize);
        }

        public async Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchPendingAsync(string term, int page = 1, int pageSize = 10)
        {
            IQueryable<Practice> query = context.Practices.Where(p => p.RegistrationStatus == Data.Enums.RegistrationFormStatus.Pending);

            return await SearchAsync(query, term, page, pageSize);
        }

        private async Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchAsync(IQueryable<Practice> query, string term, int page = 1, int pageSize = 10)
        {
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(p =>
                (p.City != null && p.City.Contains(term)) ||
                (p.Street != null && p.Street.Contains(term)) ||
                (p.Doctor.FirstName != null && p.Doctor.FirstName.Contains(term)) ||
                (p.Doctor.LastName != null && p.Doctor.LastName.Contains(term)) ||
                (p.PostalCode != null && p.PostalCode.Contains(term)));
            }

            int totalCount = await query.CountAsync();

            IEnumerable<Practice> practices = await query
                .AsNoTracking()
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (Practices: practices, TotalCount: totalCount);
        }


        public void InsertPractice(Practice practice)
        {
            context.Practices.Add(practice);
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
