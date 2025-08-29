using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("firstName")]
        public string FirstName { get; set; } = string.Empty;
        [Column("lastName")]
        public string LastName { get; set; } = string.Empty;
        [Column("bio")]
        public string Bio { get; set; } = string.Empty;
        [Column("githubUrl")]
        public string GithubUrl { get; set; } = string.Empty;
        
        [Column("role")]
        public Role Role { get; set; }
        [Column("mobileNumber")]
        public string MobileNumber { get; set; }
        
        [ForeignKey("Cohort")]
        [Column("cohort_id", TypeName = "int")]
        public int CohortId { get; set; }
        
        [NotMapped]
        public Cohort Cohort { get; set; }
        
        [NotMapped]
        public ICollection<Post> Posts { get; set; }
        
        [NotMapped]
        public ICollection<Comment> Comments { get; set; }
    }
}
