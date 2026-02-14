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

        [Column("city")]
        public string? City { get; set; }
        [Column("street")]
        public string? Street { get; set; }
        [Column("street_number")]
        public int StreetNum { get; set; }
        [Column("postal_code")]
        public string? PostalCode { get; set; }

        [Column("shift_start_time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime ShiftStartTime { get; set; }

        [Column("shift_end_time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime ShiftEndTime { get; set; }

        [Column("appointment_duration")]
        public int AppointmentDuration { get; set; }

        [Column("registration_status")]
        public RegistrationFormStatus RegistrationStatus { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
