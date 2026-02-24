using Microsoft.AspNetCore.Hosting;

namespace MedicalAppointmentWebApp.Services
{
    public class ImageService(IWebHostEnvironment webHostEnvironment) : IImageService
    {
        public async Task<string> SaveImage(IFormFile file, string directory, string[] allowedExtensions)
        {
            var wwwPath = webHostEnvironment.WebRootPath;
            var path = Path.Combine(wwwPath, directory); // wwwroot/images
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var extension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException($"Only {string.Join(",", allowedExtensions)} extensions are allowed");
            }
            
            // create the unique file name
            var newFileName = $"{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(path, newFileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return newFileName;
        }

        public void DeleteImage(string fileName, string directory)
        {
            var fullPath = Path.Combine(webHostEnvironment.WebRootPath, directory, fileName);
            if (!Path.Exists(fullPath))
            {
                throw new FileNotFoundException($"File {fileName} does not exists");
            }
            File.Delete(fullPath);
        }
    }
}
