using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("Likes")]
    public class Like : IEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("post_id")]
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        public Post Post { get; set; }
        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
