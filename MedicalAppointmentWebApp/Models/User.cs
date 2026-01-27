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

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("surname")]
        [StringLength(50)]
        public string Surname { get; set; }
        
        [Required]
        [Column("email")]
        [StringLength(254)]
        public required string Email { get; set; }
        
        [Required]
        [Column("amka")]
        [StringLength(11)]
        public required string Amka { get; set; }

        [Column("gender")]
        [Range(0, 2)]
        public int Gender { get; set; }

        [Required]
        [Column("phone")]
        public required int Phone { get; set; }

        [Column("birth_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateOnly BirthDate { get; set; }

        [Required]
        [Column("password")]
        public required string Password { get; set; }

        [Required]
        [Column("registration_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Registration Date")]
        public required DateTime registrationDate {  get; set; }
    }
}
