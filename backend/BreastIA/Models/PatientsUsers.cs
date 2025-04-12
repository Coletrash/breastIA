using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreastIA.Models
{
    [Table("PatientsUsers", Schema = "Security")]
    public class PatientsUsers
    {
        [Key]
        public int IdPatienstUsers { get; set; }

        [Required, MaxLength(50)]
        public string Email { get; set; }

        public int Age { get; set; }

        [Required, MaxLength(20)]
        public int PhoneNumber { get; set; }


        [Required, MaxLength(1)]

        public string Sex { get; set; }

        [Required]
        public byte[] Password { get; set; }
    }
}
