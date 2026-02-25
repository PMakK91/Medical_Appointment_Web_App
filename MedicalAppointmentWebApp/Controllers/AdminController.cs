using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentWebApp.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("[controller]/[action]")]
    public class AdminController(IAdminService adminService) : Controller
    {
        public async Task<IActionResult> PendingRegistrationFormsAsync(string term, int page = 1, int pageSize = 10)
        {
            DoctorSearchViewModel registrationForms = await adminService.GetPendingRegistrations(term, page, pageSize);
            return View(registrationForms);
        }

        public async Task<IActionResult> AcceptRegistrationAsync(Guid id)
        {
            Doctor? doctor = await adminService.AcceptRegistrationAsync(id);

            if (doctor is null)
            {
                return BadRequest("Doctor not found");
            }

            return RedirectToAction("PendingRegistrationForms", "Admin");
        }

        public async Task<IActionResult> RejectRegistrationAsync(Guid id)
        {
            Doctor? doctor = await adminService.RejectRegistrationAsync(id);

            if (doctor is null)
            {
                return BadRequest("Doctor not found");
            }

            return RedirectToAction("PendingRegistrationForms", "Admin");
        }

        public async Task<IActionResult> RegistrationInfoPageAsync(Guid id)
        { 

            Doctor? doctor = await adminService.ViewRegistrationAsync(id);

            if (doctor is null)
            {
                return BadRequest("Doctor not found");
            }

            return View(doctor);
        }

        public IActionResult GetWeekMock()
        {
            var data = new
            {
                weekStart = "2026-02-10",
                selectedDate = "2026-02-12",
                days = new[]
                {
                new { date="2026-02-10", dayName="Τρίτη", slots=new[] { "20:00", "20:30" } },
                new { date="2026-02-11", dayName="Τετάρτη", slots=Array.Empty<string>() },
                new { date="2026-02-12", dayName="Πέμπτη", slots=new[] { "09:00","09:30","10:00","10:30","11:00","11:30" } },
                new { date="2026-02-13", dayName="Παρασκευή", slots=new[] { "09:00","09:30","10:00","10:30","11:00","11:30" } },
                new { date="2026-02-14", dayName="Σάββατο", slots=Array.Empty<string>() },
                new { date="2026-02-15", dayName="Κυριακή", slots=Array.Empty<string>() },
                new { date="2026-02-16", dayName="Δευτέρα", slots=new[] { "09:00","09:30","10:00","10:30","11:00","11:30" } }
            }
            };

            return Ok(data);
        }
    }
}
