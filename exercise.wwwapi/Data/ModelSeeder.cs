using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Models.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data;

public static class ModelSeeder
{
    // Necessary because the seeder requires them to be constant values 
    private static string[] _passwordHashes =
    [
        "$2a$11$NlNrSkH2Uop6Nl90BHeF9udj/s5N79m9j94htBwtiwPMzoJ5EXozW", // Test1test1%
        "$2a$11$MYFrTWP6v64imGdsbibutOW/DSZiu3wg5rWR1Nm5Zjb5XBNut5HKq", // Test2test2%
        "$2a$11$JyMDiDHwh8hrcjNmp0zb8uZGFettl5dyJ3FDa3S5iOCTYnDn6GZqm", // Test3test3%
        "$2a$11$.daNf2PApH/oqC8MGCQq5uHqw2zmjmIiIB8A6WZ/nLXjbI4iuQsEW", // Test4test4%
        "$2a$11$HmeURzynKz6PqTVeZxfDIeg6MRpzI/5ZAY1GyHW0hJuNUvv7ixOOO" // Test5test5%
    ];

    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUsers(ref modelBuilder);
        SeedCredentials(ref modelBuilder);
        SeedProfiles(ref modelBuilder);
        SeedPosts(ref modelBuilder);
        SeedComments(ref modelBuilder);
        SeedCourses(ref modelBuilder);
        SeedCohorts(ref modelBuilder);
        SeedModules(ref modelBuilder);
        SeedUnits(ref modelBuilder);
        SeedExercises(ref modelBuilder);
    }

    private static void SeedCredentials(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Credential>().HasData(
            new Credential
            {
                Email = "test1@test1",
                UserId = 1,
                Username = "test1",
                PasswordHash = _passwordHashes[0],
                Role = Role.Student,
            },
            new Credential
            {
                Email = "test2@test2",
                UserId = 2,
                Username = "test2",
                PasswordHash = _passwordHashes[1],
                Role = Role.Teacher,
            },
            new Credential
            {
                Email = "test3@test3",
                UserId = 3,
                Username = "test3",
                PasswordHash = _passwordHashes[2],
                Role = Role.Student,
            },
            new Credential
            {
                Email = "test4@test4",
                UserId = 4,
                Username = "test4",
                PasswordHash = _passwordHashes[3],
                Role = Role.Teacher,
            },
            new Credential
            {
                Email = "test5@test5",
                UserId = 5,
                Username = "test5",
                PasswordHash = _passwordHashes[4],
                Role = Role.Student,
            }
        );
    }

    private static void SeedProfiles(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>().HasData(
            new Profile()
            {
                UserId = 1,
                LastName = "Jackson",
                FirstName = "Michael",
                Github = "",
                Bio = "",
                StartDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                Specialism = Specialism.Fullstack,
            },
            new Profile()
            {
                UserId = 2,
                LastName = "Jordan",
                FirstName = "Michael",
                Github = "",
                Bio = "",
                StartDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                Specialism = Specialism.Backend,
            },
            new Profile()
            {
                UserId = 3,
                LastName = "Messi",
                FirstName = "Lionel",
                Github = "",
                Bio = "",
                StartDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                Specialism = Specialism.Fullstack,
            },
            new Profile()
            {
                UserId = 4,
                LastName = "Ronaldo",
                FirstName = "Cristiano",
                Github = "",
                Bio = "",
                StartDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                Specialism = Specialism.Fullstack,
            },
            new Profile()
            {
                UserId = 5,
                LastName = "Richie",
                FirstName = "Lionel",
                Github = "",
                Bio = "",
                StartDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                Specialism = Specialism.Frontend,
            }
        );
    }

    private static void SeedUsers(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                CohortId = 1,
            },
            new User
            {
                Id = 2,
                CohortId = 2
            },
            new User
            {
                Id = 3,
                CohortId= 3
            },
            new User
            {
                Id = 4,
                CohortId = 4
            },
            new User
            {
                Id = 5,
                CohortId = 5
            }
        );
    }

    private static void SeedPosts(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().HasData(
            new Post
            {
                Id = 1,
                AuthorId = 1,
                Body = "Post 1 Body",
                Likes = 5,
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            },
            new Post
            {
                Id = 2,
                AuthorId = 2,
                Body = "Post 2 Body",
                Likes = 3,
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            },
            new Post
            {
                Id = 3,
                AuthorId = 3,
                Body = "Post 3 Body",
                Likes = 10,
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            },
            new Post
            {
                Id = 4,
                AuthorId = 4,
                Body = "Post 4 Body",
                Likes = 7,
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            },
            new Post
            {
                Id = 5,
                AuthorId = 5,
                Body = "Post 5 Body",
                Likes = 9,
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            }
        );
    }

    private static void SeedComments(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>().HasData(
            new Comment
            {
                Id = 1,
                PostId = 1,
                UserId = 1,
                Body = "Post 1 Body",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            },
            new Comment
            {
                Id = 2,
                PostId = 2,
                UserId = 2,
                Body = "Post 2 Body",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            },
            new Comment
            {
                Id = 3,
                PostId = 3,
                UserId = 3,
                Body = "Post 3 Body",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            },
            new Comment
            {
                Id = 4,
                PostId = 4,
                UserId = 4,
                Body = "Post 4 Body",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            },
            new Comment
            {
                Id = 5,
                PostId = 5,
                UserId = 5,
                Body = "Post 5 Body",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            }
        );
    }

    private static void SeedCourses(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                Id = 1,
                CourseName = "Course 1",
            },
            new Course
            {
                Id = 2,
                CourseName = "Course 2",
            },
            new Course
            {
                Id = 3,
                CourseName = "Course 3",
            },
            new Course
            {
                Id = 4,
                CourseName = "Course 4",
            },
            new Course
            {
                Id = 5,
                CourseName = "Course 5",
            }
        );
    }

    private static void SeedCohorts(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cohort>().HasData(
            new Cohort
            {
                Id = 1,
                CourseId = 1,
            },
            new Cohort
            {
                Id = 2,
                CourseId = 2,
            },
            new Cohort
            {
                Id = 3,
                CourseId = 3,
            },
            new Cohort
            {
                Id = 4,
                CourseId = 4,
            },
            new Cohort
            {
                Id = 5,
                CourseId = 5,
            }
        );
    }

    private static void SeedModules(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Module>().HasData(
            new Module
            {
                Id = 1,
                CourseId = 1,
                Title = "Course 1"
            },
            new Module
            {
                Id = 2,
                CourseId = 2,
                Title = "Course 2"
            },
            new Module
            {
                Id = 3,
                CourseId = 3,
                Title = "Course 3"
            },
            new Module
            {
                Id = 4,
                CourseId = 4,
                Title = "Course 4"
            },
            new Module
            {
                Id = 5,
                CourseId = 5,
                Title = "Course 5"
            }
        );
    }

    private static void SeedUnits(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Unit>().HasData(
            new Unit
            {
                Id = 1,
                ModuleId = 1,
                Title = "Module 1",
            },
            new Unit
            {
                Id = 2,
                ModuleId = 2,
                Title = "Module 2",
            },
            new Unit
            {
                Id = 3,
                ModuleId = 3,
                Title = "Module 3",
            },
            new Unit
            {
                Id = 4,
                ModuleId = 4,
                Title = "Module 4",
            },
            new Unit
            {
                Id = 5,
                ModuleId = 5,
                Title = "Module 5",
            }
        );
    }

    private static void SeedExercises(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>().HasData(
            new Exercise
            {
                Id = 1,
                UnitId = 1,
                Title = "Exercise 1",
                Description = "Exercise 1 description"
            },
            new Exercise
            {
                Id = 2,
                UnitId = 2,
                Title = "Exercise 2",
                Description = "Exercise 2 description"
            },
            new Exercise
            {
                Id = 3,
                UnitId = 3,
                Title = "Exercise 3",
                Description = "Exercise 3 description"
            },
            new Exercise
            {
                Id = 4,
                UnitId = 4,
                Title = "Exercise 4",
                Description = "Exercise 4 description"
            },
            new Exercise
            {
                Id = 5,
                UnitId = 5,
                Title = "Exercise 5",
                Description = "Exercise 5 description"
            }
        );
    }
}