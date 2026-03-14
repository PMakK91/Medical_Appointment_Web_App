namespace MedicalAppointmentWebApp.Services
{
    public interface IQRcodeService
    {
        byte[] GenerateQRCode(string text, int width = 250, int height = 250);
    }
}
