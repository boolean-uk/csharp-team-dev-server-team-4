using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

    [Table("user_cc")]
    public class UserCC : IEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey(nameof(CohortCourse))]
        [Column("cc_id")]
        public int CcId { get; set; }

        [ForeignKey(nameof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        public CohortCourse CohortCourse { get; set; }

        public User User { get; set; }

    }

