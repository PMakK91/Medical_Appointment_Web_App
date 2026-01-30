using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<DoctorDetails> DoctorDetails { get; set; }

        public DbSet<DoctorRegistrationForm> DoctorRegistrationForms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.DoctorDetails)
                .WithOne(d => d.User)
                .HasForeignKey<DoctorDetails>(d => d.UserId)
                .IsRequired();

            modelBuilder.Entity<DoctorRegistrationForm>().ToTable("DoctorRegistrationForms");
        }
    }
}
