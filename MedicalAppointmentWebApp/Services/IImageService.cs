namespace MedicalAppointmentWebApp.Services
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile file, string directory, string[] allowedExtensions);
        void DeleteImage(string fileName, string directory);
    }
}
