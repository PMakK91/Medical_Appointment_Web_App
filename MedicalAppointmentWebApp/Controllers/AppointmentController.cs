using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Filters;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalAppointmentWebApp.Controllers
{
    [AuthorizeRoles(UserRole.Doctor, UserRole.Patient)]
    public class AppointmentController(IAppointmentService appointmentService, IQRcodeService qRcodeService) : Controller
    {
        public async Task<IActionResult> IndexAsync(Guid id)
        {
            Appointment? appointment = await appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            byte[] qrBytes = qRcodeService.GenerateQRCode(id.ToString());
            string base64 = Convert.ToBase64String(qrBytes);
            ViewBag.QrCodeImage = $"data:image/png;base64,{base64}";

            return View(appointment);
        }

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

        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            Appointment? appointment = await appointmentService.CancelAppointmentAsync(id);

            if (appointment is null)
            {
                return BadRequest("Appointment not found");
            }

            return RedirectToAction("MyAppointments", "Appointment");
        }

        public async Task<IActionResult> CompleteAppointment(Guid id)
        {
            Appointment? appointment = await appointmentService.CompleteAppointmentAsync(id);

            if (appointment is null)
            {
                return BadRequest("Appointment not found");
            }

            return RedirectToAction(nameof(Index), new { id = appointment.Id });
        }

        public async Task<IActionResult> NoShowAppointment(Guid id)
        {
            Appointment? appointment = await appointmentService.NoShowAppointmentAsync(id);

            if (appointment is null)
            {
                return BadRequest("Appointment not found");
            }

            return RedirectToAction(nameof(Index), new { id = appointment.Id });
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
            if (request.PatientId == request.DoctorId)
            {
                return BadRequest("Δεν μπορείτε να κλείσετε ραντεβού στο γραφείο σας.");
            }

            Appointment? appointment = await appointmentService.CreateAppointmentAsync(request);
            if (appointment is null)
            {
                return BadRequest("Appointment failed to create");
            }

            return RedirectToAction(nameof(AppointmentSubmitted), new { id = appointment.Id });
        }

        public async Task<IActionResult> AppointmentSubmitted(Guid id)
        {
            Appointment? appointment = await appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            byte[] qrBytes = qRcodeService.GenerateQRCode(id.ToString());
            string base64 = Convert.ToBase64String(qrBytes);
            ViewBag.QrCodeImage = $"data:image/png;base64,{base64}";

            return View(appointment);
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
