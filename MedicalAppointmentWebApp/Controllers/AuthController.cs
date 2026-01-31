using Azure.Core;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost]
        public async Task<IActionResult> PatientRegister(RegisterRequest request)
        {
            Patient? patient = await authService.PatientRegisterAsync(request);
            if (patient == null)
            {
                return BadRequest("Email already in use...");
            }

            return Ok(patient);
        }

        public IActionResult DoctorRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegister(RegisterRequest request)
        {
            Doctor? doctor = await authService.DoctorRegisterAsync(request);
            if (doctor == null)
            {
                return BadRequest("Email or AMKA already submitted for review...");
            }

            return Ok(doctor);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
        {
            AuthResponse? response = await authService.LoginAsync(request);
            if (response is null)
            {
                return BadRequest("Wrong email or password.");
            }

            Response.Cookies.Append("access_token", response.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });
            Response.Cookies.Append("refresh_token", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            string? refreshToken = Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("No refresh token provided");

            User? user = await authService.LogoutAsync(refreshToken);

            if (user is null)
            {
                return BadRequest("Refresh Token not found.");
            }

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return RedirectToAction("Login", "Auth");
        }
    }
}
