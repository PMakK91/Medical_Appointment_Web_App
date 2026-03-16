using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using Microsoft.AspNetCore.Identity;

namespace MedicalAppointmentWebApp.Services
{
    public class UserService (IUserRepository userRepository): IUserService
    {
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            User? user = await userRepository.FindUserByIdAsync(id);
            if (user is null)
            {
                return null;
            }
            return user;
        }


        public async Task<string?> UpdatePatientInfo(Guid patientId, EditPatientProfileViewModel request)
        {
            User? user = await GetUserByIdAsync(patientId);
            if (user is null)
            {
                return "Something went wrong with the session.";
            }

            user.Email = request.Email ?? user.Email;
            user.Phone = request.Phone ?? user.Phone;
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.BirthDate = request.BirthDate ?? user.BirthDate;
            user.Amka = request.Amka ?? user.Amka;
            user.Gender = request.Gender ?? user.Gender;

            if (request.Password != null)
            {
                string hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
                user.Password = hashedPassword;
            }

            await userRepository.SaveAsync();

            return null;
        }
    }
}
