using MedicalAppointmentWebApp.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentWebApp.Models
{
    public class Practice
    {

        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // TODO Doctor or Doctos?
        [Column("doctor_id")]
        public Guid DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; } = null!;

        [Column("duration")]
        public int Duration { get; set; }

        [Column("place_id")]
        public string? PlaceId { get; set; }

        [Column("lon")]
        public double Lon { get; set; }

        [Column("lat")]
        public double Lat { get; set; }

        [Column("full_address")]
        public string? FullAddress { get; set; }

        [Column("phone")]
        public int? Phone { get; set; }

        [Column("monday_start", TypeName = "time")]
        public TimeSpan? MondayStart { get; set; }

        [Column("monday_end", TypeName = "time")]
        public TimeSpan? MondayEnd { get; set; }

        [Column("tuesday_start", TypeName = "time")]
        public TimeSpan? TuesdayStart { get; set; }

        [Column("tuesday_end", TypeName = "time")]
        public TimeSpan? TuesdayEnd { get; set; }

        [Column("wednesday_start", TypeName = "time")]
        public TimeSpan? WednesdayStart { get; set; }

        [Column("wednesday_end", TypeName = "time")]
        public TimeSpan? WednesdayEnd { get; set; }

        [Column("thursday_start", TypeName = "time")]
        public TimeSpan? ThursdayStart { get; set; }

        [Column("thursday_end", TypeName = "time")]
        public TimeSpan? ThursdayEnd { get; set; }

        [Column("friday_start", TypeName = "time")]
        public TimeSpan? FridayStart { get; set; }

        [Column("friday_end", TypeName = "time")]
        public TimeSpan? FridayEnd { get; set; }

        [Column("saturday_start", TypeName = "time")]
        public TimeSpan? SaturdayStart { get; set; }

        [Column("saturday_end", TypeName = "time")]
        public TimeSpan? SaturdayEnd { get; set; }

        [Column("sunday_start", TypeName = "time")]
        public TimeSpan? SundayStart { get; set; }

        [Column("sunday_end", TypeName = "time")]
        public TimeSpan? SundayEnd { get; set; }

        [Column("registration_status")]
        public RegistrationFormStatus RegistrationStatus { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
