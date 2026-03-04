using Azure.Core;
using MedicalAppointmentWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace MedicalAppointmentWebApp.Services
{
    public class EmailService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IEmailService
    {
        public async Task SendResetPasswordEmail(PasswordResetToken token)
        {
            string encodedToken = Uri.EscapeDataString(token.Token);
            string resetLink = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Auth/ResetPassword?token={encodedToken}";

            var htmlBody = $@"
            <h2>Αλλαγή κωδικού πρόσβασης</h2>
            <p>Πατήστε τον ακόλουθο σύνδεσμο για να αλλάξετε τον κωδικό σας:</p>
            <p>
                <a href='{resetLink}'>
                    Reset Password
                </a>
            </p>
            <p>Αν δεν ήσασταν εσείς που ζητήσατε την αλλαγή του κωδικού αγνοείστε αυτό το email.</p>
            <br />
            <p>Η ομάδα του Medicall appointment web app</p>";

            await SendEmailAsync(token.User.Email, "Αλλαγή κωδικού πρόσβασης", htmlBody);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            string smtpHost = "smtp.maileroo.com";
            int smtpPort = 587;

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(
                    configuration.GetValue<string>("Maileroo:Email"),
                    configuration.GetValue<string>("Maileroo:Password")
                ),
                EnableSsl = true
            };

            var mail = new MailMessage(
                configuration.GetValue<string>("Maileroo:Email"),
                toEmail,
                subject,
                htmlBody
            )
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);
        }
    }
}
