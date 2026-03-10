using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentWebApp.Models.ViewModels
{
    public class AvailableSlotsViewModel
    {
        public Practice Practice { get; set; }
        public DateTime StartDate { get; set; }
        public Dictionary<DateTime, List<TimeSlotViewModel>> Slots { get; set; } = new();
    }
}