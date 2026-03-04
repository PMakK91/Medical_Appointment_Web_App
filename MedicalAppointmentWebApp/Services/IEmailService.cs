using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(PasswordResetToken token);
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
}
