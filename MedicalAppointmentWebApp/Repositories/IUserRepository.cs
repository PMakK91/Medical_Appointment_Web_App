using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IUserRepository: IDisposable
    {
        Task<bool> AnyUserWithEmailAsync(string email);
        Task<bool> AnyUserWithAmkaAsync(string amka);

        Task<User?> FindUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        User? GetUserByRefreshToken(string? refreshToken);

        void Save();
        Task SaveAsync();
    }
}
