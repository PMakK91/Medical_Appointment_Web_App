using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MedicalAppointmentWebApp.Repositories
{
    public class PracticeRepository(AppDbContext context) : IPracticeRepository, IDisposable
    {
        public async Task<Practice?> GetPracticeByIdAsync(Guid id)
        {
            Practice? practice = await context.Practices.Include(p => p.Doctor).FirstOrDefaultAsync(p => p.Id == id);
            if (practice is null)
            {
                return null;
            }
            return practice;
        }

        public async Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchAllWithDoctorIdAsync(Guid doctorId, string term, int page = 1, int pageSize = 10)
        {
            IQueryable<Practice> query = context.Practices
                .Where(p => p.DoctorId == doctorId)
                .Include(p => p.Doctor);

            return await SearchAsync(query, term, page, pageSize);
        }

        public async Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchPendingAsync(string term, int page = 1, int pageSize = 10)
        {
            IQueryable<Practice> query = context.Practices
                .Where(p => p.RegistrationStatus == Data.Enums.RegistrationFormStatus.Pending)
                .Include(p => p.Doctor);

            return await SearchAsync(query, term, page, pageSize);
        }

        private async Task<(IEnumerable<Practice> Practices, int TotalCount)> SearchAsync(IQueryable<Practice> query, string term, int page = 1, int pageSize = 10)
        {
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(p =>
                (p.FullAddress != null && p.FullAddress.Contains(term)) ||
                (p.Doctor.FirstName != null && p.Doctor.FirstName.Contains(term)) ||
                (p.Doctor.LastName != null && p.Doctor.LastName.Contains(term)) );
            }

            int totalCount = await query.CountAsync();

            IEnumerable<Practice> practices = await query
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (Practices: practices, TotalCount: totalCount);
        }

        public async Task<List<Practice>> GetPracticesByDoctorIdAsync(Guid doctorId)
        {
            return await context.Practices
                .Where(p => p.DoctorId == doctorId)
                .ToListAsync();
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
