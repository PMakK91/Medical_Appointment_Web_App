using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class DoctorSearchViewModel
    {
        public required IEnumerable<Doctor> Doctors { get; set; }

        public string? Term { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages =>
            (int) Math.Ceiling((double)TotalItems / PageSize);

        public string GetDisplayName(Doctor doctor)
        {
            return doctor.Specialty.GetType()
            .GetMember(doctor.Specialty.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName() ?? doctor.Specialty.ToString();
        }
    }
}
