using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Repositories
{
    public class AppointmentRepository(AppDbContext context) : IAppointmentRepository, IDisposable
    {
        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId)
        {
            return await context.Appointments.Include(a => a.Practice).Include(a => a.Doctor).Where(a => a.PatientId == patientId).ToListAsync();
        }

        public void InsertAppointment(Appointment appointment)
        {
            context.Appointments.Add(appointment);
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
