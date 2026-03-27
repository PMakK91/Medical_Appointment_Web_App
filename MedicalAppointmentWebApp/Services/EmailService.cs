using Azure.Core;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using static System.Collections.Specialized.BitVector32;

namespace MedicalAppointmentWebApp.Services
{
    public class EmailService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IQRcodeService qRcodeService) : IEmailService
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
            <p>Η ομάδα του MyDoctorNow</p>";

            await SendEmailAsync(token.User.Email, "Αλλαγή κωδικού πρόσβασης", htmlBody, null);
        }

        public async Task SendAcceptDoctorRegistrationEmail(string doctorEmail)
        {

            var htmlBody = $@"
            <h2>Επιτυχής Εγγραφή Ιατρού</h2>
            <p>Η εγγραφή σας ολοκληρώθηκε.</p>


            <br />
            <p>Η ομάδα του MyDoctorNow</p>";

            await SendEmailAsync(doctorEmail, "Επιτυχής Εγγραφή Ιατρού", htmlBody, null);
        }

        public async Task SendRejectDoctorRegistrationEmail(string doctorEmail, string reason)
        {

            var htmlBody = $@"
            <h2>Η αίτηση εγγραφής απορρίφθηκε</h2>
            <p>Λόγος απόρριψης: {reason}</p>
            <p>Κάντε νέα αίτηση εγγραφής</p>


            <br />
            <p>Η ομάδα του MyDoctorNow</p>";

            await SendEmailAsync(doctorEmail, "Απόρριψη Αίτησης Εγγραφής", htmlBody , null);
        }



        public async Task SendAppointmentBookedEmailtoPatient(Appointment appointment)
        {
            byte[] qrBytes = qRcodeService.GenerateQRCode(appointment.Id.ToString());
            string base64 = Convert.ToBase64String(qrBytes);
            Image x = (Bitmap)((new ImageConverter()).ConvertFrom(qrBytes));


            var htmlBody = $@"
            <h2>Το ραντεβού σας με τον Ιατρό {appointment.Doctor.FirstName}  {appointment.Doctor.LastName}</h2>
             <p>Ειδικότητα Ιατρού: {appointment.Doctor.Specialty.GetDisplayName()}</p>
            <p>Αναγνωριστικό Ραντεβού: {appointment.Id}</p>
            <p>Ημέρα: {appointment.Time.ToString("dd/MM/yyyy")}</p>
            <p>Ώρα: {appointment.Time.ToString("HH:mm")}</p>
            <p>Διεύθυνση Ιατρείου: {appointment.Practice.FullAddress}</p>
            <p>Τηλέφωνο Ιατρείου: {appointment.Practice.Phone}</p>
            <br/>
            Επισυνάπτεται το QR Code που θα χρησιμοποιήσετε κατά την είσοδό σας στο ιατρείο.
            <br/>
            <p>Η ομάδα του MyDoctorNow</p>";

            await SendEmailAsync(appointment.Patient.Email, "Το Ραντεβού σας", htmlBody , x);
        }

        public async Task SendAppointmentBookedEmailtoDoctor(Appointment appointment)
        {

            var htmlBody = $@"
            <h2>Νέο ραντεβού με {appointment.Patient.FirstName}  {appointment.Patient.LastName}</h2>
            
            <p>Αναγνωριστικό Ραντεβού: {appointment.Id}</p>
            <p>AMKA Ασθενή: {appointment.Patient.Amka}</p>

            <p>Ημέρα: {appointment.Time.ToString("dd/MM/yyyy")}</p>
            <p>Ώρα: {appointment.Time.ToString("HH:mm")}</p>
            <p>Ιατρείο: {appointment.Practice.FullAddress}</p>
            <p>Email Ασθενή: {appointment.Patient.Email}</p>
            <p>Τηλέφωνο Ασθενή: {appointment.Patient.Phone}</p>


            <br />
            <p>Η ομάδα του MyDoctorNow</p>";

            await SendEmailAsync(appointment.Doctor.Email, "Νέο Ραντεβού", htmlBody , null);
        }


        public async Task SendAppointmentCanceledEmailtoPatient(Appointment appointment)
        {
          
            var htmlBody = $@"
            <h2>Το ραντεβού σας με τον Ιατρό {appointment.Doctor.FirstName}  {appointment.Doctor.LastName} ακυρώθηκε</h2>
             <p>Ειδικότητα Ιατρού: {appointment.Doctor.Specialty.GetDisplayName()}</p>
            <p>Αναγνωριστικό Ραντεβού: {appointment.Id}</p>
            <p>Ημέρα: {appointment.Time.ToString("dd/MM/yyyy")}</p>
            <p>Ώρα: {appointment.Time.ToString("HH:mm")}</p>
            <p>Διεύθυνση Ιατρείου: {appointment.Practice.FullAddress}</p>
            <p>Τηλέφωνο Ιατρείου: {appointment.Practice.Phone}</p>

            <br />
            <p>Η ομάδα του MyDoctorNow</p>";

            await SendEmailAsync(appointment.Patient.Email, "Ακύρωση Ραντεβού", htmlBody , null);
        }

        public async Task SendAppointmentCanceledEmailtoDoctor(Appointment appointment)
        {

            var htmlBody = $@"
            <h2>Το ραντεβού σας με {appointment.Patient.FirstName}  {appointment.Patient.LastName} ακυρώθηκε</h2>
            
            <p>Αναγνωριστικό Ραντεβού: {appointment.Id}</p>
            <p>AMKA Ασθενή: {appointment.Patient.Amka}</p>

            <p>Ημέρα: {appointment.Time.ToString("dd/MM/yyyy")}</p>
            <p>Ώρα: {appointment.Time.ToString("HH:mm")}</p>
            <p>Ιατρείο: {appointment.Practice.FullAddress}</p>
            <p>Email Ασθενή: {appointment.Patient.Email}</p>
            <p>Τηλέφωνο Ασθενή: {appointment.Patient.Phone}</p>


            <br />
            <p>Η ομάδα του MyDoctorNow</p>";

            await SendEmailAsync(appointment.Doctor.Email, "Ακύρωση Ραντεβού", htmlBody, null);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody, Image? attachment)
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

            using var ms = new MemoryStream();
            if (attachment != null)
            {
                
                attachment.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                var mailAttachment = new Attachment(ms, "qrcode.png", "image/png");
                mail.Attachments.Add(mailAttachment);
            }

            await client.SendMailAsync(mail);
        }
    }
}
