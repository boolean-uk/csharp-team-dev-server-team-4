using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Models.UserInfo;


namespace exercise.wwwapi.Models
{
    public class CohortCourse
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

    }
}
