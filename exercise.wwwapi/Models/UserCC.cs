using exercise.wwwapi.Models.UserInfo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("user_cc")]
    public class UserCC
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        [Column("course_id")]
        public int CourseId { get; set; }

        [ForeignKey(nameof(User))]
        [Column("user_id")]

        public int UserId { get; set; }

        public Course Course { get; set; }

        public User User { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
