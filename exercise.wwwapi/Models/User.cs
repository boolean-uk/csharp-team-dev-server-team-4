using exercise.wwwapi.Enums;
using exercise.wwwapi.Repository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User : IEntity
{
        [Key]
        [Column ("id")]
        public int Id { get; set; }
        [Required]
        [Column ("username", TypeName = "varchar(50)")]
        public string Username { get; set; }
        [Required]
        [Column ("email", TypeName = "varchar(50)")]
        public string Email { get; set; }
        [Required]
        [Column ("password_hash", TypeName = "varchar(100)")]
        public string PasswordHash { get; set; }
        [Column ("role")]
        public Role? Role { get; set; }
        [Column ("first_name", TypeName = "varchar(100)")]
        public string? FirstName { get; set; }
        [Column ("last_name", TypeName = "varchar(100)")]
        public string? LastName { get; set; }
        [Column ("mobile", TypeName = "varchar(100)")]
        public string? Mobile { get; set; }
        [Column ("github", TypeName = "varchar(100)")]
        public string? Github { get; set; }
        [Column ("bio", TypeName = "varchar(100)")]
        public string? Bio { get; set; }
        [Column ("photo_url", TypeName = "varchar(100)")]
        public string? PhotoUrl { get; set; }
        [Column ("specialism")]
        public Specialism? Specialism { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<UserExercise> User_Exercises { get; set; } = new List<UserExercise>();
        public ICollection<UserCC> User_CC { get; set; } = new List<UserCC>();

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}
