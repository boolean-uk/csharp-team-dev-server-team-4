using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("profile")]
    public class Profile
    {
        [Key]
        [ForeignKey(nameof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("github")]
        public string? Github {  get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("photo_url")]
        public string? PhotoUrl { get; set; }

        public User User { get; set; }
    }
}
