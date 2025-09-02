using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models.UserInfo
{
    [Table("profile")]
    public class Profile
    {
        [Key]
        [ForeignKey(nameof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("first_name", TypeName = "varchar(20)")] 
        public string FirstName { get; set; } = string.Empty;

        [Column("last_name", TypeName = "varchar(20)")]
        public string LastName { get; set; } = string.Empty;

        [Column("phone", TypeName = "varchar(30)")]
        public string? Phone { get; set; }

        [Column("github", TypeName = "varchar(30)")]
        public string Github { get; set; } = string.Empty;

        [Column("bio", TypeName = "varchar(1000)")]
        public string Bio { get; set; } = string.Empty;

        [Column("photo_url", TypeName = "varchar(1000)")] 
        public string? PhotoUrl { get; set; } = string.Empty;

        [JsonIgnore]
        public User User { get; set; }
    }
}
