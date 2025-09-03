using exercise.wwwapi.Models;
using exercise.wwwapi.Models.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public sealed class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelSeeder.Seed(modelBuilder);
        }
   
        public DbSet<User> Users { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Cohort> Cohorts { get; set; }
        public DbSet<CohortMember> CohortMembers { get; set; }
    }
}
