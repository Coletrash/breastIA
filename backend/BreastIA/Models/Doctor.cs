using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreastIA.Models
{
    [Table("Doctor", Schema = "Security")]
    public class Doctor
    {
        [Key]
        [Column("IdDoctor")]
        public int IdDoctor { get; set; }

        [Required]
        [Column("IdUserDoctor")]
        public int IdUserDoctor { get; set; }

        [ForeignKey("IdHospitalDoctor")]
        [Column("IdHospitalDoctor")]
        public int? IdHospitalDoctor { get; set; }

        [Required]
        [Column("NationalIdentity")]
        [StringLength(20)]
        public string NationalIdentity { get; set; }

        [Required]
        [Column("Avaible")]
        public bool Avaible { get; set; }

        [Column("DateReviewed")]
        public DateTime DateReviewed { get; set; } = DateTime.UtcNow;

        // Relaciones
        public User User { get; set; }
        public HospitalDoctor HospitalDoctor { get; set; }
    }
}
