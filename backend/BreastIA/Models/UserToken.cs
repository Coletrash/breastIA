using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreastIA.Models
{
    [Table("UserTokens", Schema = "Security")]
    public class UserToken
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public bool IsUsed { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } 
    }
}
