using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Filters;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Security.Claims;

namespace MedicalAppointmentWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController (IUserService userService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeRoles(UserRole.Patient)]
        public async Task<IActionResult> EditProfile()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            User? user = await userService.GetUserByIdAsync(Guid.Parse(userId));
            if (user is null)
            {
                return View();
            }

            EditPatientProfileViewModel model = new EditPatientProfileViewModel
            {
                User = user,
                Phone = user.Phone,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Amka = user.Amka,
                Gender = user.Gender,
            };

            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.Patient)]
        public async Task<IActionResult> EditProfile(EditPatientProfileViewModel request)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return RedirectToAction(controllerName: "Auth", actionName: "Login");
            }

            string? errorMessage = await userService.UpdatePatientInfo(Guid.Parse(userId), request);
            if (errorMessage != null)
            {
                TempData["ErrorMessage"] = "errorMessage";
                return RedirectToAction(nameof(EditProfile));
            }

            TempData["SuccessMessage"] = "Οι αλλαγές αποθηκεύτηκαν επιτυχώς.";
            return RedirectToAction(nameof(EditProfile));
        }
    }
}
