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

        public async Task<DoctorRegistrationForm?> DoctorRegisterAsync(DoctorRegisterFormRequest request)
        {
            if (await _context.DoctorRegistrationForms.AnyAsync(f => f.Email.Equals(request.Email) || f.Amka.Equals(request.Amka)))
            {
                return null;
            }

            DoctorRegistrationForm form = new DoctorRegistrationForm();
            string hashedPassword = new PasswordHasher<DoctorRegistrationForm>().HashPassword(form, request.Password);

            form.Email = request.Email;
            form.Password = hashedPassword;

            form.Name = request.Name;
            form.Lastname = request.LastName;
            form.Amka = request.Amka;
            form.Phone = request.Phone;
            form.Gender = request.Gender;
            form.BirthDate = request.BirthDate;
            form.RegistrationDate = DateTime.UtcNow;
            form.Specialty = request.Specialty;
            form.Status = RegistrationFormStatus.Pending;

            _context.DoctorRegistrationForms.Add(form);
            await _context.SaveChangesAsync();

            return form;
        }

        public async Task<User?> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email.Equals(request.Email)))
            {
                return null;
            }

            User user = new();

            string hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Email = request.Email;
            user.Password = hashedPassword;

            user.Name = request.Name;
            user.Lastname = request.LastName;
            user.Amka = request.Amka;
            user.Phone = request.Phone;
            user.Gender = request.Gender;
            user.BirthDate = DateOnly.MinValue;
            user.RegistrationDate = DateTime.Now;
            user.Role = (UserRole)request.Role;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
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
