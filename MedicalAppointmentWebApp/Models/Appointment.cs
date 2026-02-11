using MedicalAppointmentWebApp.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentWebApp.Models
{
    public class Appointment
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("practice_id")]
        public Guid PracticeId { get; set; }
        [ForeignKey("PracticeId")]
        public virtual Practice Practice { get; set; } = null!;

        [Column("patient_id")]
        public Guid PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;

        [Column("doctor_id")]
        public Guid DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; } = null!;

        [Column("time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        [Column("Status")]
        public AppointmentStatus Status { get; set; }
    }
}
