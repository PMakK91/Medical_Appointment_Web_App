using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Practice> Practices { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Doctor>().ToTable("doctors");
            modelBuilder.Entity<Patient>().ToTable("patients");
            modelBuilder.Entity<Admin>().ToTable("admins");

            modelBuilder.Entity<Practice>()
                .ToTable("practices")
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Practices)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>().ToTable("appointments");
            modelBuilder.Entity<Appointment>()
                .HasOne(p => p.Patient)
                .WithMany(a => a.Appointments)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>()
                .HasOne(d => d.Doctor)
                .WithMany(a => a.MedicalAppointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>()
                .HasOne(p => p.Practice)
                .WithMany(a => a.Appointments)
                .HasForeignKey(p => p.PracticeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(p => p.User)
                .WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
