using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models
{
    [Table("notes")]
    public class Note : IEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        [Column("user_id")]
        public int UserId { get; set; }
        
        public User User { get; set; }
        [Column("title", TypeName = "varchar(100)")]
        public string Title { get; set; }
        [Column("content", TypeName = "varchar(1000)")]
        public string Content { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
