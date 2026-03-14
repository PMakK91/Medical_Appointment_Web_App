using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalAppointmentWebApp.Data.Enums
{
    public enum AppointmentStatus
    {
        [Display(Name = "Προγραμματισμένο")]
        Scheduled = 0,
        [Display(Name = "Ολοκληρωμένο")]
        Completed = 1,
        [Display(Name = "Ακυρωμένο")]
        Canceled = 2,
        [Display(Name = "Απών")]
        No_Show = 3
    }
}
