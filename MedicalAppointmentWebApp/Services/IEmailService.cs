using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(PasswordResetToken token);
        Task SendAcceptDoctorRegistrationEmail(string email);
        Task SendRejectDoctorRegistrationEmail(string email, string reason);

        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);

        Task SendAppointmentBookedEmailtoPatient(string toEmail, Appointment appointment);
        Task SendAppointmentBookedEmailtoDoctor(string toEmail, Appointment appointment);


    }
}
