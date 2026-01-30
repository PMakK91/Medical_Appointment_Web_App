using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MedicalAppointmentWebApp.Services
{
    public class AuthService(AppDbContext _context, IConfiguration _configuration) : IAuthService
    {
        private static long TOKEN_EXPIRATION_TIME = 86400; // seconds
        private static long REFRESH_TOKEN_EXPIRATION_TIME = 604800; // seconds

        public async Task<Doctor?> DoctorRegisterAsync(DoctorRegisterFormRequest request)
        {
            if (await _context.Doctors.AnyAsync(f => f.Email.Equals(request.Email) || f.Amka.Equals(request.Amka)))
            {
                return null;
            }

            Doctor doctor = new Doctor();
            string hashedPassword = new PasswordHasher<Doctor>().HashPassword(doctor, request.Password);

            doctor.Email = request.Email;
            doctor.Password = hashedPassword;

            doctor.Name = request.Name;
            doctor.Lastname = request.LastName;
            doctor.Amka = request.Amka;
            doctor.Phone = request.Phone;
            doctor.Gender = request.Gender;
            doctor.BirthDate = request.BirthDate;
            doctor.RegistrationDate = DateTime.UtcNow;
            doctor.Specialty = request.Specialty;
            doctor.RegistrationStatus = RegistrationFormStatus.Pending;

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return doctor;
        }

        public async Task<Patient?> PatientRegisterAsync(RegisterRequest request)
        {
            if (await _context.Patients.AnyAsync(u => u.Email.Equals(request.Email)))
            {
                return null;
            }

            Patient patient = new();

            string hashedPassword = new PasswordHasher<Patient>().HashPassword(patient, request.Password);

            patient.Email = request.Email;
            patient.Password = hashedPassword;

            patient.Name = request.Name;
            patient.Lastname = request.LastName;
            patient.Amka = request.Amka;
            patient.Phone = request.Phone;
            patient.Gender = request.Gender;
            patient.BirthDate = DateOnly.MinValue;
            patient.RegistrationDate = DateTime.Now;
            patient.Role = (UserRole)request.Role;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task<AuthResponse?> LoginAsync(AuthRequest request)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(request.Email));
            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password)
                == PasswordVerificationResult.Failed)
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
            User? user = await _context.Users.FindAsync(userId);
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
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey")!));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Issuer"),
                audience: _configuration.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(TOKEN_EXPIRATION_TIME),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
