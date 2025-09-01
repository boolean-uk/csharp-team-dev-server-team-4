using System.Diagnostics;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public sealed class DataContext : DbContext
    {
        private readonly string _connectionString;
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setup Tables
            // Timestamps are generated in Postgres so it's not mixing client/server times.
            modelBuilder.Entity<Post>().Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Comment>().Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Setup Keys
                     
            // Seed ???
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
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
