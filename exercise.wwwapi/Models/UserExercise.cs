using exercise.wwwapi.Enums;
using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models;

[Table("User_Exercises")]
public class UserExercise : IEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("submission_link", TypeName = "varchar(200)")]
    public string SubmissionLink { get; set; }

    [Column("submission_time", TypeName = "date")]
    public DateTime SubmitionTime { get; set; }

    [Column("grade", TypeName = "int")]
    public int Grade { get; set; }

    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; }

    [Column("submitted")]
    public bool Submitted { get; set; }

    [ForeignKey(nameof(Exercise))]
    [Column("exercise_id")]
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }

}