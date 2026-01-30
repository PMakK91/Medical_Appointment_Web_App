namespace MedicalAppointmentWebApp.Models
{
    public class Admin: User
    {
        public Admin()
        {
            Role = Data.Enums.UserRole.Admin;
        }
    }
}
