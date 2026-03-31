using MedicalAppointmentWebApp.Models.Security;
using MedicalAppointmentWebApp.Repositories;
using MedicalAppointmentWebApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAppointmentWebApp.Data
{
    public static class StartupSeeder
    {
        public static async Task SeedAdminAsync(IAuthService authService, IUserRepository userRepo)
        {

            var email = "a@a.a";
            var firstName = "System";
            var lastName = "Admin";

            var randomPassword = "11111";

            var req = new RegisterRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = randomPassword,
            };

            var existing = await userRepo.GetUserByEmailAsync(email);

            if (existing == null)
            {
                await authService.AdminRegisterAsync(req);
            }
        }
    }
}