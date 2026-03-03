using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Filters;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalAppointmentWebApp.Controllers
{
    [AuthorizeRoles(UserRole.Doctor, UserRole.Patient)]
    public class AppointmentController(IAppointmentService appointmentService) : Controller
    {
        public async Task<IActionResult> MyAppointments()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            IEnumerable<Appointment> appointments = await appointmentService.GetAppointmentsByPatientIdAsync(Guid.Parse(userId));

            return View(appointments);
        }

        public async Task<IActionResult> VerifyAppointment(AppointmentRequestViewModel request)
        {
            AppointmentRequestViewModel? appointmentInfoViewModel = await appointmentService.GetPracticeById(request.PracticeId, request);
            if (appointmentInfoViewModel is null)
            {
                return BadRequest("Practice not found");
            }
            appointmentInfoViewModel.Time = request.Time;
            return View(appointmentInfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAppointment(AppointmentRequestViewModel request)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            request.PatientId = Guid.Parse(userId);

            Appointment? appointment = await appointmentService.CreateAppointmentAsync(request);
            if (appointment is null)
            {
                return BadRequest("Appointment failed to create");
            }

            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        public async Task<JsonResult> GetAppointmentsByDayAndPracticeIdAsync(string date, Guid practiceId)
        {
            if (date is null || date.IsWhiteSpace())
                return Json("Date was not given");

            DateTime parsedDate = DateTime.Parse(date);
            IEnumerable<AppointmentWithPatientInfoViewModel> appointments = await appointmentService.GetAppointmentsByDayAndPracticeIdAsync(parsedDate, practiceId);

            return Json(appointments);
        }

        public async Task<JsonResult> GetAppointmentsByMonthAndPracticeIdAsync(string date, Guid practiceId)
        {
            if (date is null || date.IsWhiteSpace())
                return Json("Something went wrong.");

            DateTime parsedDate = DateTime.Parse(date);
            IEnumerable<Appointment> appointments = await appointmentService.GetAppointmentsByMonthAndPracticeIdAsync(parsedDate, practiceId);

            return Json(appointments);
        }
    }
}
