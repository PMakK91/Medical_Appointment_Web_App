using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentWebApp.Services
{
    public class DoctorService(IDoctorRepository doctorRepository) : IDoctorService
    {
        public async Task<DoctorSearchViewModel> SearchAllActiveDoctors(string term, int page = 1, int pageSize = 10)
        {
            (IEnumerable<Doctor>, int) doctorsAndCount = await doctorRepository.SearchActiveAsync(term, page, pageSize);

            return new DoctorSearchViewModel
            {
                Doctors = doctorsAndCount.Item1,
                Term = term,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = doctorsAndCount.Item2
            };
        }

        public async Task<Doctor?> GetDoctorById(Guid id)
        {
            Doctor? doctor = await doctorRepository.GetDoctorByIDAsync(id);
            if (doctor is null)
            {
                return null;
            }
            return doctor;
        }
    }
}
