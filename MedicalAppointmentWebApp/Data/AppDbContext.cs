using MedicalAppointmentWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasDiscriminator<string>("role")
                .HasValue<Admin>("admin")
                .HasValue<Doctor>("doctor")
                .HasValue<Patient>("atient");
        }

        //public DbSet<User> users { get; set; }
        //public DbSet<Admin> admins { get; set; }
        //public DbSet<Doctor> doctors { get; set; }
        //public DbSet<Patient> patients { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().ToTable("users");
        //    modelBuilder.Entity<Doctor>().ToTable("doctors");
        //    modelBuilder.Entity<Patient>().ToTable("patients");
        //    modelBuilder.Entity<Admin>().ToTable("admins");
        //}
    }
}
