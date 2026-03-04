using Azure.Core;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;
using MedicalAppointmentWebApp.Models.ViewModels;
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
                ModelState.AddModelError("", "Το email χρησιμοποιείται ήδη");
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
                ModelState.AddModelError("", "Αυτό το email ή το ΑΜΚΑ έχουν ήδη υποβληθεί για εξέταση.");
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
                TempData["ErrorMessage"] = "Λάθος email ή κωδικός";
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

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            string? errorMessage = await authService.CreateResetPasswordTokenAsync(email);
            if (errorMessage != null)
            {
                TempData["ErrorMessage"] = errorMessage;
                return View();
            }
            TempData["SuccessMessage"] = "Σας έχει αποσταλεί email για την αλλαγή του κωδικού πρόσβασης σας.";
            return View();
        }

        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(controllerName: "Auth", actionName: "ForgotPassword");
            }

            NewPasswordRequestViewModel newPasswordRequestViewModel = new NewPasswordRequestViewModel
            {
                Token = token
            };
            return View(newPasswordRequestViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(NewPasswordRequestViewModel model)
        {
            if (model is null || !model.Password.Equals(model.ConfirmPassword))
            {
                TempData["ErrorMessage"] = "Οι κωδικός που δώθηκε δεν τεριάζει με την επιβεβαίωση του.";
                return RedirectToAction(controllerName: "Auth", actionName: "ForgotPassword");
            }

            PasswordResetToken? passwordResetToken = await authService.ResetPassword(model);
            if (passwordResetToken is null)
            {
                TempData["ErrorMessage"] = "Δεν βρέθηκε το ενεργό τόκεν.";
                return RedirectToAction(controllerName: "Auth", actionName: "ForgotPassword");
            }

            return RedirectToAction(controllerName: "Auth", actionName: "Login");
        }
        
        public IActionResult DoctorApplicationSubmitted()
        {
            return View();
        }
    }
}

