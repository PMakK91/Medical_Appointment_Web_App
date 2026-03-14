
using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository, IDisposable
    {
        public async Task<bool> AnyUserWithEmailAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email != null && u.Email.Equals(email));
        }

        public async Task<bool> AnyUserWithAmkaAsync(string amka)
        {
            return await context.Users.AnyAsync(u => u.Amka != null && u.Amka.Equals(amka));
        }

        public async Task<User?> FindUserByIdAsync(Guid id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(email));
        }

        public User? GetUserByRefreshToken(string? refreshToken)
        {
            return context.Users.FirstOrDefault(u => u.RefreshToken != null && u.RefreshToken.Equals(refreshToken));
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
