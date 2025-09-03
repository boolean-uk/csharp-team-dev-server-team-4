using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("modules")]
    public class Module
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("course_id")]
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }
        
        public Course Course { get; set; }
        public ICollection<Unit> Units { get; set; }
    }
}
