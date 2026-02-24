using MedicalAppointmentWebApp.Data;
using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace MedicalAppointmentWebApp.Services
{
    public class DoctorService(IDoctorRepository doctorRepository, IImageService imageService) : IDoctorService
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
            else
            {
                return "No image send";
            }

            if (request.Bio != null)
            {
                doctor.Bio = request.Bio;
                await doctorRepository.SaveAsync();
            }

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
    }
}
