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

            return RedirectToAction(controllerName: "Practice", actionName: "MyPractices");
        }

        public async Task<IActionResult> AvailableTimeSlotsAsync(Guid id, DateTime? weekStart)
        {
            WeekScheduleViewModel? weekScheduleViewModel = await practiceService.GetPracticeByIdAsync(id, weekStart);
            if (weekScheduleViewModel is null)
            {
                return BadRequest("Practice was not found.");
            }
            return PartialView("_AvailableTimeSlots", weekScheduleViewModel);
        }

        [HttpGet("{practiceId:guid}")]
        public async Task<IActionResult> Get(Guid practiceId)
        {
            var p = await practiceService.GetPracticeByIdAsync(practiceId);

            if (p == null)
                return NotFound(new { success = false, message = "Practice not found." });

            return Ok(new
            {
                p.Id,
                p.Duration,
                p.MondayStart,
                p.MondayEnd,
                p.TuesdayStart,
                p.TuesdayEnd,
                p.WednesdayStart,
                p.WednesdayEnd,
                p.ThursdayStart,
                p.ThursdayEnd,
                p.FridayStart,
                p.FridayEnd,
                p.SaturdayStart,
                p.SaturdayEnd,
                p.SundayStart,
                p.SundayEnd
            });
        }
    }
}
