using exercise.wwwapi.Models.UserInfo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class Like
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("post_id")]
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        public Post Post { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
