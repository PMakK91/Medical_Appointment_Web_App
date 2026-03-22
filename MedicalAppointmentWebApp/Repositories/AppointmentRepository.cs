using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Repositories
{
    public class AppointmentRepository(AppDbContext context) : IAppointmentRepository, IDisposable
    {
        public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
        {
            return await context.Appointments.Include(a => a.Practice).Include(a => a.Patient).Include(a => a.Doctor).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId)
        {
            return await context.Appointments.Include(a => a.Practice).Include(a => a.Doctor).Where(a => a.PatientId == patientId).OrderBy(a => a.Time).ToListAsync();
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
                .Where(a => a.PracticeId == practiceId && a.Time.Date.Equals(day.Date) && a.Status != AppointmentStatus.Canceled)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsInfoByMonthAndPracticeIdAsync(DateTime date, Guid practiceId)
        {
            return await context.Appointments
                .Where(a => a.PracticeId == practiceId && a.Time.Month.Equals(date.Month) && a.Time.Year.Equals(date.Year) && a.Status != AppointmentStatus.Canceled)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAllByPracticeAfterTodayAsync(Guid practiceId)
        {
            return await context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.PracticeId == practiceId && a.Status == AppointmentStatus.Scheduled && a.Time > DateTime.Today)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsAffectedByChangeAsync(EditPracticeViewModel editPracticeViewModel)
        {
            List<int> allowedDays = new List<int>();
            if (editPracticeViewModel.MondayStart == null) allowedDays.Add(1);
            if (editPracticeViewModel.TuesdayStart == null) allowedDays.Add(2);
            if (editPracticeViewModel.WednesdayStart == null) allowedDays.Add(3);
            if (editPracticeViewModel.ThursdayStart == null) allowedDays.Add(4);
            if (editPracticeViewModel.FridayStart == null) allowedDays.Add(5);
            if (editPracticeViewModel.SaturdayStart == null) allowedDays.Add(6);
            if (editPracticeViewModel.SundayStart == null) allowedDays.Add(0);

            var appointments = await context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.PracticeId == editPracticeViewModel.PracticeId &&
                    a.Time > DateTime.Today)
                .ToListAsync();

            return appointments
                .Where(a => allowedDays.Contains((int)a.Time.DayOfWeek))
                .ToList();
        }


        public void InsertAppointment(Appointment appointment)
        {
            context.Appointments.Add(appointment);
        }
        public void UpdateAppointmentStatus(Guid appointmentId, AppointmentStatus status)
        {
            var appointment = context.Appointments.Find(appointmentId);

            if (appointment != null)
            {
                appointment.Status = status;
                Save();
            }
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

        public async Task<(DateTime? Slot, Practice? Practice)> GetFirstAvailableSlotForDoctorAsync(Guid doctorId, double? lon, double? lat, float? areaDistance)
        {
            double maxDistanceKm = areaDistance ?? 10.0;
            double R = 6371.0; // radius of earth in km

            IQueryable<Practice> practicesQuery = context.Practices
                .Where(p => p.DoctorId == doctorId && p.RegistrationStatus == RegistrationFormStatus.Accepted);

            // Apply geo filter only if lon/lat are provided
            if (lon.HasValue && lat.HasValue)
            {
                practicesQuery = practicesQuery
                    .Where(p =>
                        R * 2 * Math.Asin(
                            Math.Sqrt(
                                Math.Pow(Math.Sin(((p.Lat - lat.Value) * Math.PI / 180) / 2), 2) +
                                Math.Cos(lat.Value * Math.PI / 180) *
                                Math.Cos(p.Lat * Math.PI / 180) *
                                Math.Pow(Math.Sin(((p.Lon - lon.Value) * Math.PI / 180) / 2), 2)
                            )
                        ) <= maxDistanceKm
                    );
            }

            var practices = await practicesQuery.ToListAsync();

            DateTime? earliest = null;
            Practice? selectedPractice = null;

            foreach (var practice in practices)
            {
                var slot = await GetFirstAvailableSlotForPracticeAsync(practice.Id);

                if (slot.HasValue && (!earliest.HasValue || slot.Value < earliest.Value))
                {
                    earliest = slot;
                    selectedPractice = practice;
                }
            }

            return (earliest, selectedPractice);
        }

        public async Task<DateTime?> GetFirstAvailableSlotForPracticeAsync(Guid practiceId)
        {
            Practice? practice = await context.Practices.AsNoTracking().FirstOrDefaultAsync(p => p.Id == practiceId);

            if (practice == null)
            {
                return null;
            }

            int durationMinutes = practice.Duration;
            DateTime now = DateTime.Now;


            // Εξετάζουμε τις επόμενες 60 ημέρες, ξεκινώντας από σήμερα.
            // offset = 0 => σήμερα,  offset = 1 => αύριο ...

            for (int offset = 0; offset < 60; offset++)
            {
                DateTime date = now.Date.AddDays(offset);

                // Βρίσκουμε το ωράριο του practice για τη συγκεκριμένη ημέρα της εβδομάδας.
                (TimeSpan? start, TimeSpan? end) = GetWorkingHours(practice, date.DayOfWeek);

                // Αν ειναι κλειστό το ιατρείο προχωράμε στην επόμενη ημέρα
                if (!start.HasValue || !end.HasValue)
                {
                    continue;
                }

                // Μετατρέπουμε το ωράριο της ημέρας σε πλήρες DateTime προσθέτοντας στην ημερομηνία του iteration την ώρα από το WorkingHours.
                // Δηλαδή από 01/01/2026 θα γίνει 01/01/2026 9:00
                DateTime dayStart = date.Add(start.Value);
                DateTime dayEnd = date.Add(end.Value);

                // Ξεκινάμε να ελέγχουμε από το πρώτο slot της ημέρας.
                DateTime slotStart = dayStart;

                // Αν είμαστε ήδη μέσα στην ημέρα, βρίσκουμε το πρώτο επόμενο slot που πρέπει να ελέξγουμε, για να μην εξετάσουμε slots που έχουν περάσει σήμερα.
                if (date.Date == now.Date && now > slotStart)
                {
                    slotStart = RoundUpToNextSlot(now, dayStart, durationMinutes);
                }

                // Ξεκινώντας από το πρώτο επόμενο slot, ελέγχουμε ανά duration, εάν υπάρχει ραντεβού στη βάση.
                // Μόλις βρούμε slot χωρίς ραντεβού, σταματάμε και επιστρέφουμε αυτό το slot.
                while (slotStart.AddMinutes(durationMinutes) <= dayEnd)
                {
                    bool hasAppointment = await context.Appointments.AnyAsync(a =>
                        a.PracticeId == practiceId &&
                        a.Time == slotStart);

                    if (!hasAppointment)
                    {
                        return slotStart;
                    }

                    slotStart = slotStart.AddMinutes(durationMinutes);
                }
            }

            return null;
        }
        private static DateTime RoundUpToNextSlot(DateTime current, DateTime dayStart, int durationMinutes)
        {
            // Αν είναι ακόμα πριν ξεκινήσει το ωράριο της βάρδιας, το πρώτο πιθανό slot είναι ακριβώς στην ώρα έναρξης.
            if (current <= dayStart)
            {
                return dayStart;
            }

            // Αν όχι, μιλάμε για σήμερα
            // Υπολογίζουμε πόσα λεπτά έχουν περάσει από την αρχή της βάρδιας.
            double minutesSinceStart = (current - dayStart).TotalMinutes;

            // Υπολογίζουμε πόσα slots έχουν περάσει από την αρχή της βάρδιας (με ceiling γιατι αν εχει επράσει έστω και λίγο ένα slot, δεν ειναι υποψήφιο) 
            int slotsPassed = (int)Math.Ceiling(minutesSinceStart / durationMinutes);

            // Προσθέτουμε στην αρχή της βάρδιας τόσα durationMinutes όσα τα slots που έχουν περάσει
            return dayStart.AddMinutes(slotsPassed * durationMinutes);
        }


        private static (TimeSpan? Start, TimeSpan? End) GetWorkingHours(Practice practice, DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => (practice.MondayStart, practice.MondayEnd),
                DayOfWeek.Tuesday => (practice.TuesdayStart, practice.TuesdayEnd),
                DayOfWeek.Wednesday => (practice.WednesdayStart, practice.WednesdayEnd),
                DayOfWeek.Thursday => (practice.ThursdayStart, practice.ThursdayEnd),
                DayOfWeek.Friday => (practice.FridayStart, practice.FridayEnd),
                DayOfWeek.Saturday => (practice.SaturdayStart, practice.SaturdayEnd),
                DayOfWeek.Sunday => (practice.SundayStart, practice.SundayEnd),
                _ => (null, null)
            };
        }

    }
}
