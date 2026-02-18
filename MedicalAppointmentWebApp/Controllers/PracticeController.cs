using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicalAppointmentWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class PracticeController(IPracticeService practiceService) : Controller
    {
        [Authorize(Roles = nameof(UserRole.Doctor))]
        public async Task<IActionResult> MyPracticesAsync(string term, int page = 1, int pageSize = 10)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            PracticeSearchViewModel practiceSearchViewModel = await practiceService.GetPracticesWithDoctorIdAsync(Guid.Parse(userId), term, page, pageSize);
            return View(practiceSearchViewModel);
        }

        [Authorize(Roles = nameof(UserRole.Doctor))]
        public IActionResult Enroll()
        {
            return View();
        }

        [Authorize(Roles = nameof(UserRole.Doctor))]
        [HttpPost]
        public async Task<IActionResult> EnrollAsync(PracticeEnrollRequest practiceEnrollRequest)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            Practice? practice = await practiceService.EnrollPracticeAsync(Guid.Parse(userId), practiceEnrollRequest);
            if (practice is null)
            {
                //return BadRequest("Practice failed to be created");
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        public async Task<IActionResult> AvailableTimeSlotsAsync(Guid id, DateTime? weekStart)
        {
            WeekScheduleViewModel weekScheduleViewModel = await practiceService.GetPracticeByIdAsync(id, weekStart);
            if (weekScheduleViewModel is null)
            {
                return BadRequest("Practice was not found.");
            }
            return PartialView("_AvailableTimeSlots", weekScheduleViewModel);
        }
    }
}
