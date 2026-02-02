using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Models.ViewModels;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicalAppointmentWebApp.Controllers
{
    public class DoctorController(IDoctorService doctorService) : Controller
    {
        public async Task<IActionResult> Index(string term, int page = 1, int pageSize = 10)
        {
            DoctorSearchViewModel model = await doctorService.SearchAllActiveDoctors(term, page, pageSize);
            return View(model);
        }

        public async Task<IActionResult> Profile(Guid doctor_id)
        {
            Doctor? doctor = await doctorService.GetDoctorById(doctor_id);
            if (doctor is null)
            {
                return BadRequest("Doctor not found.");
            }
            return View(doctor);
        }
    }
}
