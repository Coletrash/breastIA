using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreastIA.Models
{
    [Table("Reminders", Schema = "Security")]
    public class Reminder
    {
        [Key]
        public int IdReminders { get; set; }

        [Required]
        [ForeignKey("User")]
        public int IdUserReminders { get; set; }  // Este es el ID del usuario al que pertenece el recordatorio

        [Required]
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        public DateTime DateReminders { get; set; }  // Fecha del recordatorio


        public User User { get; set; } //vinculacion con el user
    }
}
