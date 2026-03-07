using Azure;
using Azure.Core;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RTools_NTS.Util;
using System.Data;
using System.Security.Claims;
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
            string? amkaCheck = authService.AmkaCheck(request.Amka!);
            if (amkaCheck != null)
            {
                //ModelState.AddModelError("", amkaCheck);
                TempData["ErrorMessage"] = amkaCheck;
                return View("Register");
            }

            string? phoneCheck = authService.PhoneCheck(request.Phone!);
            if (phoneCheck != null)
            {
                //ModelState.AddModelError("", phoneCheck);
                TempData["ErrorMessage"] = phoneCheck;
                return View("Register");
            }

            Patient? patient = await authService.PatientRegisterAsync(request);
            if (patient == null)
            {
                //ModelState.AddModelError("", "Το email χρησιμοποιείται ήδη");
                TempData["ErrorMessage"] = "Το email χρησιμοποιείται ήδη.";
                return View("Register");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegister(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View("Register", request);

            string? amkaCheck = authService.AmkaCheck(request.Amka!);
            if (amkaCheck != null)
            {
                //ModelState.AddModelError("", amkaCheck);
                TempData["ErrorMessage"] = amkaCheck;
                return View("Register", request);
            }

            string? phoneCheck = authService.PhoneCheck(request.Phone!);
            if (phoneCheck != null)
            {
                //ModelState.AddModelError("", phoneCheck);
                TempData["ErrorMessage"] = phoneCheck;
                return View("Register", request);
            }

            Doctor? doctor = await authService.DoctorRegisterAsync(request);
            if (doctor == null)
            {
                //ModelState.AddModelError("", "Αυτό το email ή το ΑΜΚΑ έχουν ήδη υποβληθεί για εξέταση.");
                TempData["ErrorMessage"] = "Το email χρησιμοποιείται ήδη.";
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

            SaveAuthCookies(response);

            return RedirectToAction("Index", "Home");
        }

        public async Task GoogleLoginAsync()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties);
        }

        [AllowAnonymous]
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Αποτυχία σύνδεσης με Google.";
                return RedirectToAction("Login");
            }

            var email = result.Principal?.FindFirstValue(ClaimTypes.Email);
            var firstName = result.Principal?.FindFirstValue(ClaimTypes.GivenName);
            var lastName = result.Principal?.FindFirstValue(ClaimTypes.Surname);

            // Sign out of the temporary cookie immediately
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            if (email is null) return RedirectToAction("Login");

            var response = await authService.GoogleLoginAsync(email);
            if (response is null)
            {
                TempData["GoogleEmail"] = email;
                TempData["GoogleFirstName"] = firstName;
                TempData["GoogleLastName"] = lastName;
                return RedirectToAction("CompleteGoogleRegistration");
            }

            SaveAuthCookies(response);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult CompleteGoogleRegistration()
        {
            var model = new RegisterRequest
            {
                Email = TempData["GoogleEmail"]?.ToString()!,
                FirstName = TempData["GoogleFirstName"]?.ToString()!,
                LastName = TempData["GoogleLastName"]?.ToString()!
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteGoogleRegistrationAsync(RegisterRequest request)
        {
            User? user = await authService.CreateGoogleUserAsync(request);
            if (user is null)
            {
                TempData["ErrorMessage"] = "Αποτυχία δημιουργίας λογαριασμού με Google.";
                return RedirectToAction("Login");
            }

            if (user is Doctor doctor)
            {
                return RedirectToAction(nameof(DoctorApplicationSubmitted));
            }

            AuthResponse? response = await authService.LoginAsync(new AuthRequest{Email = user.Email!, Password = ""});
            if (response is null)
            {
                TempData["ErrorMessage"] = "Αποτυχία σύνδεσης.";
                return RedirectToAction("Login");
            }

            SaveAuthCookies(response);

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        private void SaveAuthCookies(AuthResponse response)
        {
            Response.Cookies.Append("access_token", response.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                });

            Response.Cookies.Append("refresh_token", response.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
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
                TempData["ErrorMessage"] = "Ο κωδικός που δώθηκε δεν ταιριάζει με την επιβεβαίωση του.";
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

