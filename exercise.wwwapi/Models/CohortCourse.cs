using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace exercise.wwwapi.Models
{
    [Table("cohort_course")]
    public class CohortCourse : IEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey(nameof(Cohort))]
        [Column("cohort_id")]
        public int CohortId { get; set; }

        [ForeignKey(nameof(Course))]
        [Column("course_id")]
        public int CourseId { get; set; }

        public Cohort Cohort { get; set; }
        public Course Course { get; set; }
        public ICollection<UserCC> UserCCs { get; set; } = new List<UserCC>();



    }
}
