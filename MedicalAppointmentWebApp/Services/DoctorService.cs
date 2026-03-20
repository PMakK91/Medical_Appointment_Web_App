using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace MedicalAppointmentWebApp.Services
{


    public class DoctorService(IDoctorRepository doctorRepository, IPracticeRepository practiceRepository, IImageService imageService , IAppointmentRepository appointmentRepository) : IDoctorService
    {


        public async Task<DoctorSearchViewModel> SearchAllActiveDoctorsAsync(
            string? term, 
            double? lon,  
            double? lat, 
            int gender,
            float? areaDistance, 
            int page = 1, 
            int pageSize = 10,  
            string? sortBy = null)
        {
            (IEnumerable<Doctor> doctors, int totalCount) =  await doctorRepository.SearchActiveAsync(term!, lon, lat, gender, areaDistance, page, pageSize, sortBy);

            List<DoctorSearchResultItemViewModel> doctorItems = new();

            foreach (var doctor in doctors)
            {
                var result = await appointmentRepository.GetFirstAvailableSlotForDoctorAsync(doctor.Id , lon, lat);

                doctorItems.Add(new DoctorSearchResultItemViewModel
                {
                    Doctor = doctor,
                    FirstAvailableSlot = result.Slot,
                    Practice = result.Practice!
                });
            }

            return new DoctorSearchViewModel
            {
                Doctors = doctorItems,
                Term = term,
                Lon = lon,
                Lat = lat,
                Gender = gender,
                AreaDistance = areaDistance,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                SortBy = sortBy
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

        public async Task<string?> UpdateDoctorInfo(Guid doctorId, EditDoctorProfileViewModel request)
        {
            Doctor? doctor = await doctorRepository.GetDoctorByIDAsync(doctorId);
            if (doctor is null)
            {
                return "Something went wrong with the session.";
            }

            if (request.Image != null)
            {
                string? errorMessage = await UpdateImage(request.Image, doctor);
                if (errorMessage != null) return errorMessage;
            }

            doctor.Email = request.Email ?? doctor.Email;
            doctor.Phone = request.Phone ?? doctor.Phone;
            doctor.Bio = request.Bio ?? "";
            if (request.Password != null)
            {
                string hashedPassword = new PasswordHasher<Doctor>().HashPassword(doctor, request.Password);
                doctor.Password = hashedPassword;
            }

            await doctorRepository.SaveAsync();


            return null;
        }
        private async Task<string?> UpdateImage(IFormFile imageFile, Doctor doctor)
        {
            if (imageFile.Length > 1024 * 1024)
            {
                return "Image size can not exceed 1 mb.";
            }

            try
            {
                doctor.ImageUrl = await imageService.SaveImage(imageFile, "Images", new string[] { ".jpg", ".jpeg", ".png" });
                await doctorRepository.SaveAsync();
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
        }

        public Task<List<Practice>> GetPracticesAsync(Guid doctorId) => practiceRepository.GetPracticesByDoctorIdAsync(doctorId);


    }
}