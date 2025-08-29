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
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Cohort>().ToTable("cohorts");
            
            // Setup Keys
            
            // Setup Relations
            
            // Cohorts <=> Course
            modelBuilder.Entity<Cohort>()
                .HasOne(cohort => cohort.Course)
                .WithMany(course => course.Cohorts)
                .HasForeignKey(cohort => cohort.CourseId);
            
            // Cohort <=> Users
            modelBuilder.Entity<Cohort>()
                .HasMany(cohort => cohort.Users)
                .WithOne(cohort => cohort.Cohort)
                .HasForeignKey(cohort => cohort.CohortId);
            
            // Post <=> Comments
            modelBuilder.Entity<Post>()
                .HasMany(post => post.Comments)
                .WithOne(comment => comment.Post)
                .HasForeignKey(comment => comment.PostId);
            
            // Comments <=> User
            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.User)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.UserId);
            
            // Posts <=> User
            modelBuilder.Entity<Post>()
                .HasOne(post => post.User)
                .WithMany(user => user.Posts)
                .HasForeignKey(post => post.UserId);
            
            // Seed ???
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }
   
        public DbSet<User> Users { get; set; }
        public DbSet<Cohort> Cohorts { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
