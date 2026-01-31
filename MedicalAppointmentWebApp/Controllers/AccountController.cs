using Microsoft.AspNetCore.Mvc;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.ViewModels;
using MedicalAppointmentWebApp.Helpers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        var specialties = Enum.GetValues(typeof(Specialty))
            .Cast<Specialty>()
            .Select(s => new SpecialtyItemVm
            {
                Value = (int)s,
                Text = s.GetDisplayName()
            })
            .ToList();

        // either pass directly OR put into a bigger Register VM (recommended)
        return View(specialties);
    }
}