using MedicalAppointmentWebApp.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentWebApp.Models
{
    public class DoctorRegistrationForm
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }
        [Column("Lastname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Required]
        [Column("email")]
        [StringLength(254)]
        public string? Email { get; set; }

        [Required]
        [Column("amka")]
        [StringLength(11)]
        public string? Amka { get; set; }

        [Column("gender")]
        [Range(0, 2)]
        public int Gender { get; set; }

        [Required]
        [Column("phone")]
        public string? Phone { get; set; }

        [Column("birth_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateOnly? BirthDate { get; set; }

        [Required]
        [Column("password")]
        public string? Password { get; set; }

        [Required]
        [Column("registration_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Registration Date")]
        public DateTime RegistrationDate { get; set; }

        [Column("specialty")]
        public Specialty? Specialty { get; set; }

        [Column("status")]
        public RegistrationFormStatus? Status { get; set; }
    }
}
