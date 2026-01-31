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
                ModelState.AddModelError("", "Email already in use...");
                return View("Register");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegister(RegisterRequest request)
        {
            Doctor? doctor = await authService.DoctorRegisterAsync(request);
            if (doctor == null)
            {
                ModelState.AddModelError("", "Email or AMKA already submitted for review...");
                return View("Register");
            }

            return RedirectToAction("Index", "Home");
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
                ModelState.AddModelError("", "Wrong email or password");
                return View();
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
