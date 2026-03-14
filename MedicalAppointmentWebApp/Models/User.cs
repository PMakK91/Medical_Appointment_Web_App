using MedicalAppointmentWebApp.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentWebApp.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("first_name")]
        [StringLength(100)]
        public string? FirstName { get; set; }
        [Column("last_name")]
        [StringLength(100)]
        public string? LastName { get; set; }
        
        [Required]
        [Column("email")]
        [StringLength(254)]
        public string? Email { get; set; }
        
        [Column("amka")]
        [StringLength(11)]
        public string? Amka { get; set; }

        [Column("gender")]
        [Range(0, 2)]
        public int Gender { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("birth_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateOnly? BirthDate { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Required]
        [Column("registration_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Registration Date")]
        public DateTime RegistrationDate {  get; set; }

        [Required]
        [Column("role")]
        public UserRole Role { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry_time")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    }
}
