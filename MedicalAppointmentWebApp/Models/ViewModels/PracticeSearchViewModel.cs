using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class PracticeSearchViewModel
    {
        public required IEnumerable<Practice> Practices { get; set; }

        public string? Term { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
