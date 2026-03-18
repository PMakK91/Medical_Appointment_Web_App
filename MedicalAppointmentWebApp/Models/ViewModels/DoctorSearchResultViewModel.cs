using MedicalAppointmentWebApp.Models;

public class DoctorSearchResultItemViewModel
{
    public Doctor Doctor { get; set; }
    public DateTime? FirstAvailableSlot { get; set; }

    public Practice Practice { get; set; }
}