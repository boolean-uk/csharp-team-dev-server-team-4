using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using exercise.wwwapi.Enums;

namespace exercise.wwwapi.Models.UserInfo
{
    [Table("credentials")]
    public class Credential
    {
        [Key]
        [ForeignKey(nameof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("email", TypeName = "varchar(100)")]
        public string Email { get; set; } = string.Empty;

        [Required] [Column("username", TypeName = "varchar(100)")] 
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("password_hash", TypeName = "varchar(100)")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("role")]
        
        public Role Role { get; set; }
        
        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}
