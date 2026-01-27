using MedicalAppointmentWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace MedicalAppointmentWebApp.Models
{
    public class Doctor : User
    {
        [Column("image")]
        public string? ImageUrl {  get; set; }

        [Column("bio")]
        [StringLength(500)]
        public string? Bio { get; set; }

        [Column("speciality")]
        public Specialty Specialty { get; set; }
    }
}
