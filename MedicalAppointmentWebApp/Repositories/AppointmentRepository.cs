using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Repositories
{
    public class AppointmentRepository(AppDbContext context) : IAppointmentRepository, IDisposable
    {
        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId)
        {
            return await context.Appointments.Include(a => a.Practice).Include(a => a.Doctor).Where(a => a.PatientId == patientId).ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByTimeAsync(DateTime time, Guid practiceId)
        {
            return await context.Appointments.FirstOrDefaultAsync(a => a.Time == time && a.PracticeId == practiceId);
        }

        public async Task<IEnumerable<Appointment>> GetAllByPracticeAndTimeFrameAsync(DateTime startTime, DateTime endTime, Guid practiceId)
        {
            return await context.Appointments
                .Where(a => a.PracticeId == practiceId && a.Time > startTime && a.Time < endTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDayAndPracticeIdAsync(DateTime day, Guid practiceId)
        {
            return await context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.PracticeId == practiceId && a.Time.Date.Equals(day.Date))
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentWithPatientInfoViewModel>> GetAppointmentsInfoByDayAndPracticeIdAsync(DateTime day, Guid practiceId)
        {
            return await context.Appointments.Select(a => new AppointmentWithPatientInfoViewModel
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PracticeId = a.PracticeId,
                    Time = a.Time,
                    Status = a.Status,
                    PatientId = a.PatientId,
                    PatientFirstName = a.Patient.FirstName,
                    PatientLastName = a.Patient.LastName
                })
                .Where(a => a.PracticeId == practiceId && a.Time.Date.Equals(day.Date))
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsInfoByMonthAndPracticeIdAsync(DateTime date, Guid practiceId)
        {
            return await context.Appointments
                .Where(a => a.PracticeId == practiceId && a.Time.Month.Equals(date.Month) && a.Time.Year.Equals(date.Year))
                .ToListAsync();
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
