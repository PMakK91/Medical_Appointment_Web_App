using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
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


        public async Task<IActionResult> PendingPracticesAsync(string term, int page = 1, int pageSize = 10)
        {
            PracticeSearchViewModel practiceSearchViewModel = await adminService.SearchPendingPractices(term, page, pageSize);
            return View(practiceSearchViewModel);
        }
        
        public async Task<IActionResult> AcceptPracticeAsync(Guid id)
        {
            Practice? practice = await adminService.AcceptPracticeAsync(id);
            if (practice is null)
            {
                return BadRequest("Practice not found");
            }
            return RedirectToAction("PendingPractices", "Admin");
        }

        public async Task<IActionResult> RejectPracticeAsync(Guid id)
        {
            Practice? practice = await adminService.RejectPracticeAsync(id);
            if (practice is null)
            {
                return BadRequest("Practice not found");
            }
            return RedirectToAction("PendingPractices", "Admin");
        }

        public async Task<IActionResult> PracticeInfoPageAsync(Guid id)
        {
            Practice? practice = await adminService.ViewPracticeByIdAsync(id);
            if (practice is null)
            {
                return BadRequest("Practice not found");
            }
            return View(practice);
        }
    }
}
