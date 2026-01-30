using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentWebApp.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("[controller]/[action]")]
    public class AdminController(IAdminService adminService) : Controller
    {
        public async Task<IActionResult> PendingRegistrationFormsAsync()
        {
            IEnumerable<DoctorRegistrationForm> registrationForms = await adminService.GetPendingRegistrations();
            return View(registrationForms);
        }
    }
}
