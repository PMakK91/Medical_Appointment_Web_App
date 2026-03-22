using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Operation.Distance;

namespace MedicalAppointmentWebApp.Repositories
{
    public class DoctorRepository(AppDbContext context, IAppointmentRepository appointmentRepository) : IDoctorRepository, IDisposable
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


        public async Task<bool> AnyDoctorWithEmailOrAmkaOrLicenseAsync(string email, string amka, string license)
        {
            return await context.Doctors.AnyAsync(d =>
                (d.Email != null && d.Email.Equals(email)) ||
                (d.Amka != null && d.Amka.Equals(amka)) ||
                (d.LicenseNumber != null && d.LicenseNumber.Equals(license)));
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

        public async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchActiveAsync(string term, double? lon, double? lat, int gender, float? areaDistance, int page, int pageSize , string? sortBy)
        {
            return await SearchAsync(term, lon, lat, gender, areaDistance, page, pageSize, RegistrationFormStatus.Accepted , sortBy);
        }

        public async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchPendingAsync(string term, int page, int pageSize)
        {
            return await SearchAsync(term, null, null, 0, null, page, pageSize, RegistrationFormStatus.Pending , "name");
        }

        private async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> SearchAsync(
            string? term,
            double? lon,
            double? lat,
            int gender,
            float? areaDistance,
            int page,
            int pageSize,
            RegistrationFormStatus status,
            string? sortBy)
        {

            IQueryable<Doctor> query = context.Doctors
                .Include(d => d.Practices)
                .Where(d => d.RegistrationStatus == status);

            // FILTER BY GENDER
            if (gender == 1 || gender == 2)
            {
                query = query.Where(d => d.Gender == gender);
            }

            // SEARCH TERM
            if (!string.IsNullOrWhiteSpace(term))
            {
                List<Specialty> specialties = Enum.GetValues(typeof(Specialty))
                    .Cast<Specialty>()
                    .Where(s => RemoveTones(s.GetDisplayName()).ToLower().Contains(RemoveTones(term).ToLower()))
                    .ToList();

                query = query
                    .Where(d =>
                    (d.FirstName != null && d.FirstName.Contains(term)) ||
                    (d.LastName != null && d.LastName.Contains(term)) ||
                    (d.Email != null && d.Email.Contains(term)) ||
                    specialties.Contains(d.Specialty)
                );
            }

            // FILTER BY SEARCH AREA
            if (lon != null && lat != null)
            {
                double maxDistanceKm = areaDistance ?? 10.0;
                double R = 6371.0; // radius of earth in km

                var doctorIds = await context.Practices
                    .Where(p =>
                        p.RegistrationStatus == RegistrationFormStatus.Accepted &&
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



            // SORT BY FIRST AVAILABLE
            if (sortBy == "first_available")
            {
                var doctorsList = await query
                    .AsNoTracking()
                    .ToListAsync();

                var doctorsWithAvailability = new List<(Doctor Doctor, DateTime? FirstAvailable, Practice Practice)>();

                foreach (var doctor in doctorsList)
                {
                    var result = await appointmentRepository.GetFirstAvailableSlotForDoctorAsync(doctor.Id, lon, lat , areaDistance);

                    doctorsWithAvailability.Add((doctor!, result.Slot!, result.Practice!));
                }

                var sortedDoctors = doctorsWithAvailability
                    .OrderBy(x => x.FirstAvailable ?? DateTime.MaxValue)
                    .ThenBy(x => x.Doctor.LastName)
                    .ThenBy(x => x.Doctor.FirstName)
                    .ToList();

                int totalCount = sortedDoctors.Count;

                var pagedDoctors = sortedDoctors
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => x.Doctor)
                    .ToList();

                return (pagedDoctors, totalCount);
            }

            // NORMAL SORTING
            query = sortBy switch
            {
                "name_asc" => query.OrderBy(d => d.FirstName).ThenBy(d => d.LastName),
                "name_desc" => query.OrderByDescending(d => d.FirstName).ThenByDescending(d => d.LastName),
                "specialty_asc" => query.OrderBy(d => d.Specialty).ThenBy(d => d.LastName),
                "specialty_desc" => query.OrderByDescending(d => d.Specialty).ThenBy(d => d.LastName),
                _ => query.OrderBy(d => d.LastName).ThenBy(d => d.FirstName)
            };

            int total = await query.CountAsync();

            var doctors = await query
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (doctors, total);
        }

        private static string RemoveTones(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text
                .Replace("ά", "α")
                .Replace("έ", "ε")
                .Replace("ή", "η")
                .Replace("ί", "ι")
                .Replace("ό", "ο")
                .Replace("ύ", "υ")
                .Replace("ώ", "ω")
                .Replace("Ά", "Α")
                .Replace("Έ", "Ε")
                .Replace("Ή", "Η")
                .Replace("Ί", "Ι")
                .Replace("Ό", "Ο")
                .Replace("Ύ", "Υ")
                .Replace("Ώ", "Ω");
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
