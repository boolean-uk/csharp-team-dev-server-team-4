using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("exercise")]
    public class Exercise
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("unit_id")]
        [ForeignKey(nameof(Unit))]
        public int UnitId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }


        public Unit Unit { get; set; } = null!;
    }
}
