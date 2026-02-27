using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Filters;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalAppointmentWebApp.Controllers
{
    public class DoctorController(IDoctorService doctorService) : Controller
    {
        public async Task<IActionResult> Index(string term, int page = 1, int pageSize = 10)
        {
            DoctorSearchViewModel model = await doctorService.SearchAllActiveDoctors(term, page, pageSize);
            return View(model);
        }

        public async Task<IActionResult> Profile(Guid doctor_id)
        {
            Doctor? doctor = await doctorService.GetDoctorById(doctor_id);
            if (doctor is null)
            {
                return BadRequest("Doctor not found.");
            }
            return View(doctor);
        }

        [AuthorizeRoles(UserRole.Doctor)]
        public async Task<IActionResult> EditProfile()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            Doctor? doctor = await doctorService.GetDoctorById(Guid.Parse(userId));
            if (doctor is null)
            {
                return View();
            }

            EditDoctorProfileViewModel model = new EditDoctorProfileViewModel
            {
                Doctor = doctor,
                Bio = doctor.Bio
            };

            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.Doctor)]
        public async Task<IActionResult> EditProfile(EditDoctorProfileViewModel request)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            string? errorMessage = await doctorService.UpdateDoctorInfo(Guid.Parse(userId), request);
            if (errorMessage != null)
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction(controllerName: "Doctor", actionName: "EditProfile");
            }

            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        public async Task<IActionResult> CalendarAsync()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            Doctor? doctor = await doctorService.GetDoctorById(Guid.Parse(userId));
            if (doctor is null)
            {
                return View();
            }

            return View(doctor);
        }

        public async Task<IActionResult> OfficesAsync()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            Doctor? doctor = await doctorService.GetDoctorById(Guid.Parse(userId));
            if (doctor is null)
            {
                return View();
            }

            return View(doctor);
        }

        public IActionResult AddOffice()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return View();
        }
    }
}
