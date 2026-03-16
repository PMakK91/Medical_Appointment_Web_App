using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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

        public async Task<IActionResult> AvailableTimeSlotsAllAsync(Guid practiceId)
        {
            AvailableSlotsViewModel? availableSlotsViewModel = await practiceService.GetPracticeAvailableSlotsAsync(practiceId);
            if (availableSlotsViewModel is null)
            {
                return BadRequest("Practice was not found.");
            }
            return PartialView("_AvailableTimeSlots", availableSlotsViewModel);
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

        [HttpPost]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            Practice? practice = await practiceService.GetPracticeByIdAsync(id);
            if (practice is null)
            {
                return RedirectToAction(actionName: "MyPractices", controllerName: "Practice");
            }

            return View(new EditPracticeViewModel
            {
                Practice = practice
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditVerificationAsync(EditPracticeViewModel editPracticeViewModel)
        {
            Practice? practice = await practiceService.GetPracticeByIdAsync(editPracticeViewModel.PracticeId);
            if (practice is null)
            {
                ViewBag.ErrorMessage = "Το γραφείου που προσπαθείτε να τροποποιήσετε δεν βρέθηκε.";
                return RedirectToAction(actionName: "MyPractices", controllerName: "Practice");
            }

            editPracticeViewModel.Practice = practice;
            editPracticeViewModel.Appointments = await practiceService.GetAppointmentsAffectedByChangeAsync(editPracticeViewModel);

            return View(editPracticeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPracticeAsync(EditPracticeViewModel editPracticeViewModel)
        {
            Practice? practice = await practiceService.GetPracticeByIdAsync(editPracticeViewModel.PracticeId);
            if (practice is null)
            {
                ViewBag.ErrorMessage = "Το γραφείου που προσπαθείτε να τροποποιήσετε δεν βρέθηκε.";
                return RedirectToAction(actionName: "MyPractices", controllerName: "Practice");
            }

            editPracticeViewModel.Practice = practice;
            editPracticeViewModel.Appointments = await practiceService.GetAppointmentsAffectedByChangeAsync(editPracticeViewModel);

            // TODO αν δεν επηρεάζεται κανένα, προχωράμε χωρίς επιβεβαίωση

            string? errorMessage = await practiceService.UpdatePracticeInfoAsync(editPracticeViewModel);
            if (errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
                return RedirectToAction(actionName: "MyPractices", controllerName: "Practice");
            }

            ViewBag.SuccessMessage = "Επιτυχία αλλαγής στοιχείων γραφείου.";
            return RedirectToAction(actionName: "MyPractices", controllerName: "Practice");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            string? message = await practiceService.DeletePracticeAsync(id);
            
            ViewBag.Message = message;
            return RedirectToAction(actionName: "MyPractices", controllerName: "Practice");
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
