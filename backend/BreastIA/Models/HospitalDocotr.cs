using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreastIA.Models
{
    [Table("HospitalDoctor", Schema = "Security")]
    public class HospitalDoctor
    {
        [Key]
        [Column("IdHospital")]
        public int IdHospital { get; set; }

        [Required]
        [Column("Name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Column("Specialty")]
        [StringLength(50)]
        public string Specialty { get; set; }

        [Required]
        [Column("ConnetionHospital")]
        [StringLength(40)]
        public string ConnectionHospital { get; set; }

        [Required]
        [Column("PhoneNumber")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [Column("Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [Column("Street")]
        [StringLength(40)]
        public string Street { get; set; }

        [Required]
        [Column("City")]
        [StringLength(40)]
        public string City { get; set; }

        [Required]
        [Column("Country")]
        [StringLength(40)]
        public string Country { get; set; }
    }
}
