using MedicalAppointmentWebApp.Data.Enums;
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
            practice.Duration = practiceEnrollRequest.Duration;
            practice.PlaceId = practiceEnrollRequest.PlaceId;
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

        //private async Task<WeekScheduleViewModel> BuildWeekScheduleAsync(Practice practice, DateTime weekStart)
        //{
        //    List<TimeSlotViewModel> slots = new List<TimeSlotViewModel>();

        //    IEnumerable<Appointment> appointments = await appointmentRepository.GetAllByPracticeAndTimeFrameAsync(weekStart, weekStart.AddDays(7), practice.Id);

        //    for (int day = 0; day < 7; day++)
        //    {
        //        DateTime currentDay = weekStart.AddDays(day);

        //        TimeSpan? shiftStart = day switch
        //        {
        //            0 => practice.MondayStart,
        //            1 => practice.TuesdayStart,
        //            2 => practice.WednesdayStart,
        //            3 => practice.ThursdayStart,
        //            4 => practice.FridayStart,
        //            5 => practice.SaturdayStart,
        //            6 => practice.SundayStart,
        //            _ => null
        //        };

        //        TimeSpan? shiftEnd = day switch
        //        {
        //            0 => practice.MondayEnd,
        //            1 => practice.TuesdayEnd,
        //            2 => practice.WednesdayEnd,
        //            3 => practice.ThursdayEnd,
        //            4 => practice.FridayEnd,
        //            5 => practice.SaturdayEnd,
        //            6 => practice.SundayEnd,
        //            _ => null
        //        };

        //        // If closed / not set, skip day
        //        if (!shiftStart.HasValue || !shiftEnd.HasValue)
        //            continue;

        //        DateTime dayStart = currentDay.Date + shiftStart.Value;
        //        DateTime dayEnd = currentDay.Date + shiftEnd.Value;

        //        while (dayStart < dayEnd)
        //        {
        //            var slotEnd = dayStart.AddMinutes(practice.Duration);

        //            // don’t create a slot that goes past the end of shift
        //            if (slotEnd > dayEnd)
        //                break;

        //            TimeSlotViewModel slot = new TimeSlotViewModel
        //            {
        //                Start = dayStart,
        //                End = slotEnd,
        //                IsBooked = appointments.Any(a => a.Time == dayStart)
        //            };

        //            slots.Add(slot);

        //            dayStart = slotEnd;
        //        }
        //    }

        //    return new WeekScheduleViewModel
        //    {
        //        Practice = practice,
        //        WeekStartDate = weekStart,
        //        Slots = slots
        //    };
        //}

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
