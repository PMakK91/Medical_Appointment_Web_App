using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;

namespace MedicalAppointmentWebApp.Services
{
    public class AppointmentService(IAppointmentRepository appointmentRepository,
        IPracticeRepository practiceRepository,
        IDoctorRepository doctorRepository,
        IUserRepository userRepository) : IAppointmentService
    {
        public async Task<AppointmentRequestViewModel?> GetPracticeById(Guid practiceId, AppointmentRequestViewModel request)
        {
            Practice? practice = await practiceRepository.GetPracticeByIdAsync(practiceId);
            if (practice is null)
            {
                return null;
            }

            request.Practice = practice;
            request.Doctor = practice.Doctor;
            return request;
        }

        public async Task<Appointment?> CreateAppointmentAsync(AppointmentRequestViewModel request)
        {
            Practice? practice = await practiceRepository.GetPracticeByIdAsync(request.PracticeId);
            if (practice is null)
            {
                return null;
            }

            Doctor? doctor = await doctorRepository.GetDoctorByIDAsync(request.DoctorId);
            if (doctor is null)
            {
                return null;
            }

            User? patient = await userRepository.FindUserByIdAsync(request.PatientId);
            if (patient is null)
            {
                return null;
            }

            Appointment? appointment = new Appointment();

            appointment.Practice = practice;
            appointment.Doctor = doctor;
            appointment.Patient = patient;
            appointment.Time = request.Time;
            appointment.Status = Data.Enums.AppointmentStatus.Scheduled;

            appointmentRepository.InsertAppointment(appointment);
            await appointmentRepository.SaveAsync();

            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(Guid patientId)
        {
            return await appointmentRepository.GetAppointmentsByPatientIdAsync(patientId);
        }
    }
}
