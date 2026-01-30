using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicalAppointmentWebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController(IAuthService authService) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult DoctorRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegister(DoctorRegisterFormRequest request)
        {
            DoctorRegistrationForm? form = await authService.DoctorRegisterAsync(request);
            if (form == null)
            {
                return BadRequest("Email or AMKA already submitted for review...");
            }

            return Ok(form);
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
