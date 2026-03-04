using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Repositories
{
    public interface IPasswordResetTokenRepository : IDisposable
    {
        Task<PasswordResetToken?> GetPasswordResetTokenByTokenAsync(string token);
        Task<IEnumerable<PasswordResetToken>> GetActiveByUserAsync(Guid userId);

        void InsertPasswordResetToken(PasswordResetToken passwordResetToken);

        void Save();
        Task SaveAsync();
    }
}
