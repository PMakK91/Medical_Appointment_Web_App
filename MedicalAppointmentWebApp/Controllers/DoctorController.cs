using MedicalAppointmentWebApp.Models;
using MedicalAppointmentWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicalAppointmentWebApp.Controllers
{
    public class DoctorController(IDoctorService doctorService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            IEnumerable<Doctor> doctors = await doctorService.GetAllActiveDoctors();
            return View(doctors);
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
