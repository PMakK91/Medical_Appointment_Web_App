using MedicalAppointmentWebApp.Models;

namespace MedicalAppointmentWebApp.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(PasswordResetToken token);
        Task SendAcceptDoctorRegistrationEmail(string email);
        Task SendRejectDoctorRegistrationEmail(string email, string reason);

        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);

        Task SendAppointmentBookedEmailtoPatient(Appointment appointment);
        Task SendAppointmentBookedEmailtoDoctor(Appointment appointment);

        Task SendAppointmentCanceledEmailtoPatient(Appointment appointment);
        Task SendAppointmentCanceledEmailtoDoctor(Appointment appointment);


    }
}
