using Azure.Core;
using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentWebApp.Services
{
    public class AuthService(
        IConfiguration configuration, 
        IDoctorRepository doctorRepository, 
        IUserRepository userRepository,
        IPatientRepository patientRepository,
        IAdminRepository adminRepository,
        IPasswordResetTokenRepository passwordResetTokenRepository,
        IEmailService emailService) : IAuthService
    {
        private static long TOKEN_EXPIRATION_TIME = 86400; // seconds = 24 hours
        private static long REFRESH_TOKEN_EXPIRATION_TIME = 28800; // seconds = 8 hours

        private static long PASSWORD_RESET_TOKEN_EXPIRATION_TIME = 1800; // seconds = 0.5 hours

        public async Task<Doctor?> DoctorRegisterAsync(RegisterRequest request)
        {
            if (await doctorRepository.AnyDoctorWithEmailOrAmkaOrLicenseAsync(request.Email, request.Amka!, request.LicenseNumber!))
            {
                return null;
            }

            Doctor doctor = new Doctor();
            doctor.Email = request.Email;

            if (request.Password != null)
            {
                string hashedPassword = new PasswordHasher<Doctor>().HashPassword(doctor, request.Password);
                doctor.Password = hashedPassword;
            }

            doctor.FirstName = request.FirstName;
            doctor.LastName = request.LastName;
            doctor.Amka = request.Amka;
            doctor.Phone = request.Phone;
            doctor.Gender = request.Gender;
            doctor.BirthDate = request.BirthDate;
            doctor.RegistrationDate = DateTime.UtcNow;
            doctor.Specialty = (Specialty)request.Specialty!;
            doctor.LicenseNumber = request.LicenseNumber;
            doctor.RegistrationStatus = RegistrationFormStatus.Pending;

            doctorRepository.InsertDoctor(doctor);
            await doctorRepository.SaveAsync();

            return doctor;
        }

        public async Task<Patient?> PatientRegisterAsync(RegisterRequest request)
        {
            if (await userRepository.AnyUserWithEmailOrAmkaAsync(request.Email, request.Amka!))
            {
                return null;
            }

            Patient? patient = new();

            if (request.Password != null)
            {
                string hashedPassword = new PasswordHasher<Patient>().HashPassword(patient, request.Password);
                patient.Password = hashedPassword;
            }

            patient = AssignValues(patient, request) as Patient;

            if (patient is null) return null;

            patientRepository.InsertPatient(patient);
            await patientRepository.SaveAsync();

            return patient;
        }

        public async Task<Admin?> AdminRegisterAsync(RegisterRequest request)
        {
            if (await userRepository.AnyUserWithEmailAsync(request.Email))
            {
                return null;
            }

            Admin? admin = new Admin();
            
            string hashedPassword = new PasswordHasher<Admin>().HashPassword(admin, request.Password);
            admin.Password = hashedPassword;

            admin = AssignValues(admin, request) as Admin;

            if (admin is null) return null;

            adminRepository.InsertAdmin(admin);
            await adminRepository.SaveAsync();

            return admin;
        }

        private User AssignValues(User user, RegisterRequest request)
        {
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Amka = request.Amka;
            user.Phone = request.Phone;
            user.Gender = request.Gender;
            user.BirthDate = request.BirthDate;
            user.RegistrationDate = DateTime.Now;

            return user;
        }

        public async Task<User?> LogoutAsync(string? refreshToken)
        {
            User? user = userRepository.GetUserByRefreshToken(refreshToken);

            if (user is null)
            {
                return null;    
            }
            
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await userRepository.SaveAsync();

            return user;
        }

        public async Task<AuthResponse?> LoginAsync(AuthRequest request)
        {
            User? user = await userRepository.GetUserByEmailAsync(request.Email);
            if (user is null) return null;

            if (string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(request.Password)) return null;

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password ?? "", request.Password);

            if (verificationResult == PasswordVerificationResult.Failed) return null;

            if (user is Doctor doctor && doctor.RegistrationStatus != RegistrationFormStatus.Accepted)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        private async Task<AuthResponse> CreateTokenResponse(User user)
        {
            return new AuthResponse
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            User? user = await ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (user is null) 
            { 
                return null; 
            }

            return await CreateTokenResponse(user);
        }

        private async Task<User?> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            User? user = await userRepository.FindUserByIdAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) 
            {
                return null;
            }

            return user;
        }
        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            string refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddSeconds(REFRESH_TOKEN_EXPIRATION_TIME);
            await userRepository.SaveAsync();
            return refreshToken;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("SecretKey")!));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Issuer"),
                audience: configuration.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(TOKEN_EXPIRATION_TIME),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<AuthResponse?> GoogleLoginAsync(string email)
        {
            User? user = await userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return null;

            return await CreateTokenResponse(user);
        }

        public async Task<User?> CreateGoogleUserAsync(RegisterRequest request)
        {
            if (request.UserRole.Equals(UserRole.Patient))
            {
                return await PatientRegisterAsync(request);
            }

            return await DoctorRegisterAsync(request);
        }

        public async Task<string?> CreateResetPasswordTokenAsync(string email)
        {
            User? user = await userRepository.GetUserByEmailAsync(email);
            if (user is null) 
                return "Το email δεν βρέθηκε";

            // find and de-activate the active tokens
            IEnumerable<PasswordResetToken> activePasswordResetTokens = await passwordResetTokenRepository.GetActiveByUserAsync(user.Id);
            foreach(PasswordResetToken token in activePasswordResetTokens)
            {
                token.ExpiryDate = DateTime.UtcNow.AddDays(-2);
            }
            await passwordResetTokenRepository.SaveAsync();

            PasswordResetToken passwordResetToken = new PasswordResetToken
            {
                Token = CreatePasswordResetToken(),
                UserId = user.Id,
                User = user,
                ExpiryDate = DateTime.UtcNow.AddSeconds(PASSWORD_RESET_TOKEN_EXPIRATION_TIME),
                IsUsed = false
            };

            passwordResetTokenRepository.InsertPasswordResetToken(passwordResetToken);
            await passwordResetTokenRepository.SaveAsync();

            await emailService.SendResetPasswordEmail(passwordResetToken);

            return null;
        }

        private string CreatePasswordResetToken()
        {
            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            string hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

            return hashedToken;
        }

        public async Task<PasswordResetToken?> ResetPassword(NewPasswordRequestViewModel model)
        {
            if (model is null)
                return null;

            PasswordResetToken? passwordResetToken = await passwordResetTokenRepository.GetPasswordResetTokenByTokenAsync(model.Token);
            if (passwordResetToken is null || passwordResetToken.IsUsed)
                return null;

            User? user = passwordResetToken.User;
            if (user is null)
                return null;

            string hashedPassword = new PasswordHasher<User>().HashPassword(user, model.Password);
            user.Password = hashedPassword;
            passwordResetToken.User?.Password = hashedPassword;
            await userRepository.SaveAsync();

            passwordResetToken.IsUsed = true;
            await passwordResetTokenRepository.SaveAsync();

            return passwordResetToken;
        }

        public string? AmkaCheck(string amka)
        {
            if (amka.Length != 11)
            {
                return "Το ΑΜΚΑ πρέπει να έχει 11 αριθμούς";
            }

            if (!amka.All(char.IsDigit))
            {
                return "Το ΑΜΚΑ μπορεί να αποτελείται μόνο από αριθμούς";
            }

            return null;
        }

        public string? PhoneCheck(string phone)
        {
            if (phone.Length != 10)
            {
                return "Το τηλέφωνο απαιτείται να έχει 10 ψηφία";
            }

            if (!phone.All(char.IsDigit))
            {
                return "Το τηλέφωνο μπορεί να αποτελείται μόνο από αριθμούς";
            }

            return null;
        }
    }
}
