using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentWebApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFoundPage()
        {
            return View("404");
        }

        [Route("Error/403")]
        public IActionResult UnauthorizedPage()
        {
            return View("403");
        }
    }
}
