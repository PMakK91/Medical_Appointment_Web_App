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
    public class DoctorController(IDoctorService doctorService, IQRcodeService qRcodeService) : Controller
    {
        
        public async Task<IActionResult> Search(DoctorSearchViewModel request)
        {
            if (request is null)
            {
                return BadRequest("Something went wrong");
            }

            DoctorSearchViewModel model = await doctorService.SearchAllActiveDoctorsAsync(
                request.Term!, request.Lon, request.Lat, request.CurrentPage, request.PageSize);

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
                TempData["ErrorMessage"] = "errorMessage";
                return RedirectToAction(nameof(EditProfile));
            }

            TempData["SuccessMessage"] = "Οι αλλαγές αποθηκεύτηκαν επιτυχώς.";
            return RedirectToAction(nameof(EditProfile));
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

        [AuthorizeRoles(UserRole.Doctor)]

        public IActionResult ScanQR()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.Doctor)]
        public IActionResult ScanQR(IFormFile qrFile)
        {
            if (qrFile == null || qrFile.Length == 0)
            {
                return BadRequest("No file selected.");
            }

            try
            {
                using (var stream = qrFile.OpenReadStream())
                {
                    ViewBag.ScanResult = qRcodeService.ScanQRCode(stream);
                }
            }
            catch (Exception ex)
            {
                //ViewBag.ScanResult = "Error processing image: " + ex.Message;
                return BadRequest("Error processing image: " + ex.Message);
            }

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.Doctor)]
        public IActionResult ScanQRFromCamera(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image)) return BadRequest("No file selected.");

            string base64Data = base64Image.Substring(base64Image.IndexOf(",") + 1);
            byte[] binData = Convert.FromBase64String(base64Data);

            using (var stream = new MemoryStream(binData))
            {
                ViewBag.ScanResult = qRcodeService.ScanQRCode(stream);
            }

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
