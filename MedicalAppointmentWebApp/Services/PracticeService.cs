using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;

namespace MedicalAppointmentWebApp.Services
{
    public class PracticeService(IDoctorRepository doctorRepository, 
        IPracticeRepository practiceRepository, 
        IAppointmentRepository appointmentRepository,
        IEmailService emailService) : IPracticeService
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
            practice.Duration = practiceEnrollRequest.Duration;
            practice.FullAddress = practiceEnrollRequest.FullAddress;
            practice.Lon = practiceEnrollRequest.Lon;
            practice.Lat = practiceEnrollRequest.Lat;
            practice.Phone= practiceEnrollRequest.Phone;
            practice.MondayStart = practiceEnrollRequest.MondayStart;
            practice.MondayEnd = practiceEnrollRequest.MondayEnd;
            practice.TuesdayStart = practiceEnrollRequest.TuesdayStart;
            practice.TuesdayEnd = practiceEnrollRequest.TuesdayEnd;
            practice.WednesdayStart = practiceEnrollRequest.WednesdayStart;
            practice.WednesdayEnd = practiceEnrollRequest.WednesdayEnd;
            practice.ThursdayStart = practiceEnrollRequest.ThursdayStart;
            practice.ThursdayEnd = practiceEnrollRequest.ThursdayEnd;
            practice.FridayStart = practiceEnrollRequest.FridayStart;
            practice.FridayEnd = practiceEnrollRequest.FridayEnd;
            practice.SaturdayStart = practiceEnrollRequest.SaturdayStart;
            practice.SaturdayEnd = practiceEnrollRequest.SaturdayEnd;
            practice.SundayStart = practiceEnrollRequest.SundayStart;
            practice.SundayEnd = practiceEnrollRequest.SundayEnd;

            practice.RegistrationDate = DateTime.Now;

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

        public async Task<WeekScheduleViewModel?> GetPracticeByIdAsync(Guid id, DateTime? weekStart)
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

        public async Task<IEnumerable<Appointment>> GetAppointmentsAffectedByChangeAsync(EditPracticeViewModel editPracticeViewModel)
        {
            return await appointmentRepository.GetAppointmentsAffectedByChangeAsync(editPracticeViewModel);
        }

        public async Task<string?> UpdatePracticeInfoAsync(EditPracticeViewModel editPracticeViewModel)
        {
            Practice? practice = await GetPracticeByIdAsync(editPracticeViewModel.PracticeId);
            if (practice is null)
            {
                return "Το γραφείου που προσπαθείτε να τροποποιήσετε δεν βρέθηκε";
            }

            foreach (Appointment appointment in editPracticeViewModel.Appointments)
            {
                appointment.Status = AppointmentStatus.Canceled;
                await emailService.SendAppointmentCanceledEmailtoPatient(appointment);
            }

            practice.Duration = editPracticeViewModel.Duration;
            practice.Phone = editPracticeViewModel.Phone;

            practice.MondayStart = editPracticeViewModel.MondayStart;
            practice.MondayEnd = editPracticeViewModel.MondayEnd;
            practice.TuesdayStart = editPracticeViewModel.TuesdayStart;
            practice.TuesdayEnd = editPracticeViewModel.TuesdayEnd;
            practice.WednesdayStart = editPracticeViewModel.WednesdayStart;
            practice.WednesdayEnd = editPracticeViewModel.WednesdayEnd;
            practice.ThursdayStart = editPracticeViewModel.ThursdayStart;
            practice.ThursdayEnd = editPracticeViewModel.ThursdayEnd;
            practice.FridayStart = editPracticeViewModel.FridayStart;
            practice.FridayEnd = editPracticeViewModel.FridayEnd;
            practice.SaturdayStart = editPracticeViewModel.SaturdayStart;
            practice.SaturdayEnd = editPracticeViewModel.SaturdayEnd;
            practice.SundayStart = editPracticeViewModel.SundayStart;
            practice.SundayEnd = editPracticeViewModel.SundayEnd;
            await practiceRepository.SaveAsync();

            return null;
        }

        public async Task<string?> DeletePracticeAsync(Guid id, Guid doctorId)
        {
            Practice? practice = await GetPracticeByIdAsync(id);
            if (practice is null)
                return "Αποτυχία διαγραφής Γραφείου.";

            if (practice.DoctorId != doctorId)
                return "Το γραφείο δεν σας ανήκει.";

            List<Appointment> appointments = await appointmentRepository.GetAllByPracticeAfterTodayAsync(practice.Id);

            foreach (Appointment appointment in appointments)
            {
                appointment.Status = AppointmentStatus.Canceled;
                await emailService.SendAppointmentCanceledEmailtoPatient(appointment);
            }

            practice.RegistrationStatus = RegistrationFormStatus.Rejected;
            practice.IsDeleted = true;
            practice.DeletedTime = DateTime.UtcNow;
            await practiceRepository.SaveAsync();

            return "Το Γραφείο σας διαγράφηκε με επιτυχία.";
        }


        public async Task<AvailableSlotsViewModel?> GetPracticeAvailableSlotsAsync(Guid id)
        {
            Practice? practice = await practiceRepository.GetPracticeByIdAsync(id);
            if (practice is null)
            {
                return null;
            }

            DateTime startDate = DateTime.Now;
            

            return await BuildCalendarAsync(practice, startDate);
        }

        private async Task<WeekScheduleViewModel> BuildWeekScheduleAsync(Practice practice, DateTime weekStart)
        {
            List<TimeSlotViewModel>[] slots = new List<TimeSlotViewModel>[7];

            IEnumerable<Appointment> appointments = await appointmentRepository.GetAllByPracticeAndTimeFrameAsync(weekStart, weekStart.AddDays(7), practice.Id);

            for (int day = 0; day < 7; day++)
            {
                slots[day] = new List<TimeSlotViewModel>();

                DateTime currentDay = weekStart.AddDays(day);

                if (currentDay.Date < DateTime.UtcNow.Date)
                    continue;

                TimeSpan? shiftStart = currentDay.DayOfWeek switch
                {
                    DayOfWeek.Monday => practice.MondayStart,
                    DayOfWeek.Tuesday => practice.TuesdayStart,
                    DayOfWeek.Wednesday => practice.WednesdayStart,
                    DayOfWeek.Thursday => practice.ThursdayStart,
                    DayOfWeek.Friday => practice.FridayStart,
                    DayOfWeek.Saturday => practice.SaturdayStart,
                    DayOfWeek.Sunday => practice.SundayStart,
                    _ => null
                };

                TimeSpan? shiftEnd = currentDay.DayOfWeek switch
                {
                    DayOfWeek.Monday => practice.MondayEnd,
                    DayOfWeek.Tuesday => practice.TuesdayEnd,
                    DayOfWeek.Wednesday => practice.WednesdayEnd,
                    DayOfWeek.Thursday => practice.ThursdayEnd,
                    DayOfWeek.Friday => practice.FridayEnd,
                    DayOfWeek.Saturday => practice.SaturdayEnd,
                    DayOfWeek.Sunday => practice.SundayEnd,
                    _ => null
                };

                // If closed / not set, skip day
                if (!shiftStart.HasValue || !shiftEnd.HasValue)
                    continue;

                DateTime dayStart = currentDay.Date + shiftStart.Value;
                DateTime dayEnd = currentDay.Date + shiftEnd.Value;

                while (dayStart < dayEnd)
                {
                    var slotEnd = dayStart.AddMinutes(practice.Duration);

                    // don’t create a slot that goes past the end of shift
                    if (slotEnd > dayEnd)
                        break;

                    TimeSlotViewModel slot = new TimeSlotViewModel
                    {
                        Start = dayStart,
                        End = slotEnd,
                        IsBooked = appointments.Any(a => a.Time == dayStart)
                    };

                    slots[day].Add(slot);

                    dayStart = slotEnd;
                }
            }

            return new WeekScheduleViewModel
            {
                Practice = practice,
                WeekStartDate = weekStart,
                Slots = slots
            };
        }

        private async Task<AvailableSlotsViewModel> BuildCalendarAsync(Practice practice, DateTime startDate)
        {
            DateTime rangeStart = startDate.Date;
            DateTime rangeEnd = rangeStart.AddMonths(2);


            DateTime now = DateTime.Now;

            IEnumerable<Appointment> appointments =
                await appointmentRepository.GetAllByPracticeAndTimeFrameAsync(
                    rangeStart,
                    rangeEnd,
                    practice.Id);

            HashSet<DateTime> bookedTimes = appointments
                .Select(a => a.Time)
                .ToHashSet();

            var slots = new Dictionary<DateTime, List<TimeSlotViewModel>>();

            for (DateTime currentDay = rangeStart; currentDay < rangeEnd; currentDay = currentDay.AddDays(1))
            {
                if (currentDay.Date < now.Date)
                    continue;

                TimeSpan? shiftStart = currentDay.DayOfWeek switch
                {
                    DayOfWeek.Monday => practice.MondayStart,
                    DayOfWeek.Tuesday => practice.TuesdayStart,
                    DayOfWeek.Wednesday => practice.WednesdayStart,
                    DayOfWeek.Thursday => practice.ThursdayStart,
                    DayOfWeek.Friday => practice.FridayStart,
                    DayOfWeek.Saturday => practice.SaturdayStart,
                    DayOfWeek.Sunday => practice.SundayStart,
                    _ => null
                };

                TimeSpan? shiftEnd = currentDay.DayOfWeek switch
                {
                    DayOfWeek.Monday => practice.MondayEnd,
                    DayOfWeek.Tuesday => practice.TuesdayEnd,
                    DayOfWeek.Wednesday => practice.WednesdayEnd,
                    DayOfWeek.Thursday => practice.ThursdayEnd,
                    DayOfWeek.Friday => practice.FridayEnd,
                    DayOfWeek.Saturday => practice.SaturdayEnd,
                    DayOfWeek.Sunday => practice.SundayEnd,
                    _ => null
                };

                if (!shiftStart.HasValue || !shiftEnd.HasValue)
                    continue;

                DateTime dayStart = currentDay.Date.Add(shiftStart.Value);
                DateTime dayEnd = currentDay.Date.Add(shiftEnd.Value);

                var daySlots = new List<TimeSlotViewModel>();

                while (dayStart < dayEnd)
                {
                    DateTime slotEnd = dayStart.AddMinutes(practice.Duration);

                    if (slotEnd > dayEnd)
                        break;

                    // Hide slots that already started/passed today
                    if (dayStart <= now)
                    {
                        dayStart = slotEnd;
                        continue;
                    }

                    if (!bookedTimes.Contains(dayStart))
                    {
                        daySlots.Add(new TimeSlotViewModel
                        {
                            Start = dayStart,
                            End = slotEnd,
                            IsBooked = false
                        });
                    }

                    dayStart = slotEnd;
                }

                if (daySlots.Count > 0)
                {
                    slots[currentDay.Date] = daySlots;
                }
            }

            return new AvailableSlotsViewModel
            {
                Practice = practice,
                StartDate = rangeStart,
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
