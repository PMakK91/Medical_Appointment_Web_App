using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class DoctorSearchViewModel
    {
        public required IEnumerable<DoctorSearchResultItemViewModel> Doctors { get; set; }

        public string? Term { get; set; }

        public double? Lon { get; set; }
        public double? Lat { get; set; }
        public string? FullAddress { get; set; }


        public string? SortBy { get; set; }
        public int Gender { get; set; }
        public float? AreaDistance { get; set; }


        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
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
