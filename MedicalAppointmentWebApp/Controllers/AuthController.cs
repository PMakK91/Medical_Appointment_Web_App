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
            string? amkaCheck = authService.AmkaCheck(request.Amka);
            if (amkaCheck != null)
            {
                ModelState.AddModelError("", amkaCheck);
                return View("Register");
            }

            string? phoneCheck = authService.PhoneCheck(request.Phone);
            if (phoneCheck != null)
            {
                ModelState.AddModelError("", phoneCheck);
                return View("Register");
            }

            Patient? patient = await authService.PatientRegisterAsync(request);
            if (patient == null)
            {
                ModelState.AddModelError("", "Email already in use...");
                return View("Register");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> DoctorRegister(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View("Register", request);

            string? amkaCheck = authService.AmkaCheck(request.Amka);
            if (amkaCheck != null)
            {
                ModelState.AddModelError("", amkaCheck);
                return View("Register", request);
            }

            string? phoneCheck = authService.PhoneCheck(request.Phone);
            if (phoneCheck != null)
            {
                ModelState.AddModelError("", phoneCheck);
                return View("Register", request);
            }

            Doctor? doctor = await authService.DoctorRegisterAsync(request);
            if (doctor == null)
            {
                ModelState.AddModelError("", "Email or AMKA already submitted for review.");
                return View("Register", request);
            }

            return RedirectToAction(nameof(DoctorApplicationSubmitted));
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
        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult DoctorApplicationSubmitted()
        {
            return View();
        }
    }
  
    }

