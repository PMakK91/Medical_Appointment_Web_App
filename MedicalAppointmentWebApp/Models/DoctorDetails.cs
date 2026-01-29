using MedicalAppointmentWebApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace MedicalAppointmentWebApp.Models
{
    public class DoctorDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DoctorId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [Required]
        public virtual User User { get; set; }

        [Column("image")]
        public string? ImageUrl {  get; set; }

        [Column("bio")]
        [StringLength(500)]
        public string? Bio { get; set; }

        [Column("specialty")]
        public Specialty Specialty { get; set; }
    }
}
