using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Repositories
{
    public class PasswordResetTokenRepository(AppDbContext context) : IPasswordResetTokenRepository, IDisposable
    {

        public async Task<PasswordResetToken?> GetPasswordResetTokenByTokenAsync(string token)
        {
            return await context.PasswordResetTokens.Include(p => p.User).FirstOrDefaultAsync(p => p.Token.Equals(token));
        }

        public async Task<IEnumerable<PasswordResetToken>> GetActiveByUserAsync(Guid userId)
        {
            return await context.PasswordResetTokens.Where(p => p.UserId == userId && p.ExpiryDate < DateTime.Now).ToListAsync();
        }

        public void InsertPasswordResetToken(PasswordResetToken passwordResetToken)
        {
            context.PasswordResetTokens.Add(passwordResetToken);
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }



        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
