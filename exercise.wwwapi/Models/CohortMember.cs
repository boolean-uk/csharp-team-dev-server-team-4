using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Models.UserInfo;

namespace exercise.wwwapi.Models
{
    [Table("cohortmembers")]
    public class CohortMember
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Column("cohort_id")]
        [ForeignKey(nameof(Cohort))]
        public int CohortId { get; set; }
        public User User { get; set; }
        public Cohort Cohort { get; set; }
    }
}
