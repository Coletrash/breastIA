using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreastIA.Models
{
    [Table("MammographicTests", Schema = "Security")]
    public class MammographicTest
    {
        [Key]
        public int IdMammographicTests { get; set; }

        [ForeignKey("PatientsUsers")]
        public int IdPatienst { get; set; }

        [Required, MaxLength(40)]
        public string TypeTest { get; set; }

        [Required, MaxLength(240)]
        public string Img { get; set; }

        [Required, MaxLength(50)]
        [RegularExpression(@"^(Cancer|No Cancer|Tumor)$")]
        public string Result { get; set; }

        [MaxLength(50)]
        public string Comments { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public virtual PatientsUsers Patient { get; set; }
    }
}

