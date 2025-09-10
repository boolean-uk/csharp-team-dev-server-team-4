using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using exercise.wwwapi.Enums;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Models.UserInfo;

[Table("credentials")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class Credential
{
    [Key]
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("email", TypeName = "varchar(100)")]
    public string Email { get; set; }

    [Required] 
    [Column("username", TypeName = "varchar(100)")] 
    public string Username { get; set; }

    [Required]
    [Column("password_hash", TypeName = "varchar(100)")]
    public string PasswordHash { get; set; }

    [Required]
    [Column("role")]
    public Role Role { get; set; }
        
    [JsonIgnore]
    public User User { get; set; }
}