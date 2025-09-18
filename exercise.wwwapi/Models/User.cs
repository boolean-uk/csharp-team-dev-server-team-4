using exercise.wwwapi.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User
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
        public Role Role { get; set; }
        [Column ("first_name", TypeName = "varchar(100)")]
        public string FirstName { get; set; }
        [Column ("last_name", TypeName = "varchar(100)")]
        public string LastName { get; set; }
        [Column ("mobile", TypeName = "varchar(100)")]
        public string Mobile { get; set; }
        [Column ("github", TypeName = "varchar(100)")]
        public string Github { get; set; }
        [Column ("bio", TypeName = "varchar(100)")]
        public string Bio { get; set; }
        [Column ("photo_url", TypeName = "varchar(100)")]
        public string PhotoUrl { get; set; }
        [Column ("specialism")]
        public Specialism Specialism { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<User_Exercise> User_Exercises { get; set; }
        public ICollection<User_CC> User_CC { get; set; }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}
