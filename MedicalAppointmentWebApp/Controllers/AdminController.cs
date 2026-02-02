using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentWebApp.Controllers
{
    //[Authorize(Roles = nameof(UserRole.Admin))]
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class AdminController(IAdminService adminService) : Controller
    {
        public async Task<IActionResult> PendingRegistrationFormsAsync()
        {
            IEnumerable<Doctor> registrationForms = await adminService.GetPendingRegistrations();
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
    }
}
