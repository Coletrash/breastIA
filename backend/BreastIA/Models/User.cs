using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreastIA.Models
{
    [Table("Users", Schema = "Security")] // Especifica la tabla y el esquema
    public class User
    {
        [Key]
        [Column("IdUsers")]
        public int IdUsers { get; set; }

        [Required]
        [StringLength(40)]
        public string FullName { get; set; } = string.Empty;

        [Required]

        [StringLength(40)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("Email")]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("Password")]
        public string Password { get; set; } = string.Empty; 

        [Column("Country")]
        [StringLength(20)]
        public string? Country { get; set; } 

        [Column("City")]
        [StringLength(20)]
        public string? City { get; set; }

        [Required]
        [Column("Street")]
        [StringLength(30)]
        public string Street { get; set; } = string.Empty;

        [Column("CreateAt")]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

      


        [Column("PhoneNumber")]
        public decimal? PhoneNumber { get; set; } 

        [Column("ImgUsers", TypeName = "text")]
        public string? ImgUsers { get; set; } 
    }
}
