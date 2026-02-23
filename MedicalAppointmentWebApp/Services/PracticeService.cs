using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;

namespace MedicalAppointmentWebApp.Services
{
    public class PracticeService(IDoctorRepository doctorRepository, IPracticeRepository practiceRepository, IAppointmentRepository appointmentRepository) : IPracticeService
    {

        public async Task<Practice?> EnrollPracticeAsync(Guid doctorId, PracticeEnrollRequest practiceEnrollRequest)
        {
            Doctor? doctor = await doctorRepository.GetDoctorByIDAsync(doctorId);
            if (doctor == null)
            {
                return null;
            }

            Practice practice = new Practice();

            practice.DoctorId = doctor.Id;
            practice.City = practiceEnrollRequest.City;
            practice.Street = practiceEnrollRequest.Street;
            practice.StreetNum = practiceEnrollRequest.StreetNum;
            practice.PostalCode= practiceEnrollRequest.PostalCode;
            practice.ShiftStartTime = practiceEnrollRequest.ShiftStartTime;
            practice.ShiftEndTime = practiceEnrollRequest.ShiftEndTime;
            practice.AppointmentDuration = practiceEnrollRequest.AppointmentDuration;

            practice.RegistrationStatus = Data.Enums.RegistrationFormStatus.Pending;

            practiceRepository.InsertPractice(practice);
            await practiceRepository.SaveAsync();

            return practice;
        }

        public async Task<PracticeSearchViewModel> GetPracticesWithDoctorIdAsync(Guid doctorId, string term, int page, int pageSize)
        {
            (IEnumerable<Practice>, int) practicesAndCount = await practiceRepository.SearchAllWithDoctorIdAsync(doctorId, term, page, pageSize);

            return new PracticeSearchViewModel
            {
                Practices = practicesAndCount.Item1,
                Term = term,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = practicesAndCount.Item2
            };
        }

        public async Task<Practice?> GetPracticeByIdAsync(Guid id)
        {
            Practice? practice = await practiceRepository.GetPracticeByIdAsync(id);
            if (practice is null)
            {
                return null;
            }
            return practice;
        }

        public async Task<WeekScheduleViewModel> GetPracticeByIdAsync(Guid id, DateTime? weekStart)
        {
            Practice? practice = await practiceRepository.GetPracticeByIdAsync(id);
            if (practice is null)
            {
                return null;
            }

            if (!weekStart.HasValue)
            {
                weekStart = DateTime.Now;
            }

            return await BuildWeekScheduleAsync(practice, StartOfWeek((DateTime)weekStart));
        }

        private async Task<WeekScheduleViewModel> BuildWeekScheduleAsync(Practice practice, DateTime weekStart)
        {
            List<TimeSlotViewModel> slots = new List<TimeSlotViewModel>();

            IEnumerable<Appointment> appointments = await appointmentRepository.GetAllByPracticeAndTimeFrameAsync(weekStart, weekStart.AddDays(7), practice.Id);

            for (int day = 0; day < 7; day++)
            {
                DateTime currentDay = weekStart.AddDays(day);
                DateTime dayStart = currentDay.Date + practice.ShiftStartTime.TimeOfDay;
                DateTime dayEnd = currentDay.Date + practice.ShiftEndTime.TimeOfDay;

                while (dayStart < dayEnd)
                {
                    TimeSlotViewModel slot = new TimeSlotViewModel
                    {
                        Start = dayStart,
                        End = dayStart.AddMinutes(practice.AppointmentDuration),
                        IsBooked = false
                    };

                    Appointment? appointment = appointments.FirstOrDefault(a => a.Time == dayStart);
                    if (appointment != null) slot.IsBooked = true;

                    slots.Add(slot);

                    dayStart = dayStart.AddMinutes(practice.AppointmentDuration);
                }
            }

            return new WeekScheduleViewModel
            {
                Practice = practice,
                WeekStartDate = weekStart,
                Slots = slots
            };
        }

        private DateTime StartOfWeek(DateTime dateTime)
        {
            int difference = dateTime.DayOfWeek - DayOfWeek.Monday;
            if (difference < 0)
            {
                difference += 7;
            }
            return dateTime.AddDays(-1 * difference).Date;
        }
    }
}
