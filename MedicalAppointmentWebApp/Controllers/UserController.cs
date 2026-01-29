using MedicalAppointmentWebApp.Data.Enums;
using MedicalAppointmentWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentWebApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
