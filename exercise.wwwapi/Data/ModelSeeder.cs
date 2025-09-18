using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Models.UserInfo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data;

public static class ModelSeeder
{
    private static readonly DateTime _seedTime = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

    private static string[] _passwordHashes =
    [
        "$2a$11$NlNrSkH2Uop6Nl90BHeF9udj/s5N79m9j94htBwtiwPMzoJ5EXozW", // Test1test1%
        "$2a$11$MYFrTWP6v64imGdsbibutOW/DSZiu3wg5rWR1Nm5Zjb5XBNut5HKq", // Test2test2%
        "$2a$11$JyMDiDHwh8hrcjNmp0zb8uZGFettl5dyJ3FDa3S5iOCTYnDn6GZqm", // Test3test3%
        "$2a$11$.daNf2PApH/oqC8MGCQq5uHqw2zmjmIiIB8A6WZ/nLXjbI4iuQsEW", // Test4test4%
        "$2a$11$HmeURzynKz6PqTVeZxfDIeg6MRpzI/5ZAY1GyHW0hJuNUvv7ixOOO"  // Test5test5%
    ];

    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUsers(ref modelBuilder);
        SeedPosts(ref modelBuilder);
        SeedComments(ref modelBuilder);
        SeedLikes(ref modelBuilder);
        SeedCourses(ref modelBuilder);
        SeedCohorts(ref modelBuilder);
        SeedModules(ref modelBuilder);
        SeedUnits(ref modelBuilder);
        SeedExercises(ref modelBuilder);
        SeedNotes(ref modelBuilder);
    }

   


    private static void SeedUsers(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "test1",
                Email = "test1@test1",              
                PasswordHash = _passwordHashes[0],
                Role = Role.Student,
                FirstName = "Lionel",
                LastName = "Richie",
                Mobile = "1234567890",
                Github = "",
                Bio = "",               
                Specialism = Specialism.Frontend
            },
            new User
            {
                Id = 2,
                Username = "test2",
                Email = "test2@test2",
                PasswordHash = _passwordHashes[1],
                Role = Role.Teacher,
                FirstName = "Michael",
                LastName = "Jordan",
                Mobile = "1234123",
                Github = "",
                Bio = "",
                Specialism = Specialism.Backend
            },
            new User
            {
                Id = 3,
                Username = "test3",
                Email = "test3@test3",
                PasswordHash = _passwordHashes[2],
                Role = Role.Student,
                FirstName = "Michael",
                LastName = "Johansen",
                Mobile = "55555555",
                Github = "",
                Bio = "",
                Specialism = Specialism.Frontend
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
                CreatedAt = _seedTime,
            },
            new Post
            {
                Id = 2,
                AuthorId = 2,
                Body = "Post 2 Body",
                CreatedAt = _seedTime,
            },
            new Post
            {
                Id = 3,
                AuthorId = 1,
                Body = "Post 3 Body",
                CreatedAt = _seedTime,
            },
            new Post
            {
                Id = 4,
                AuthorId = 3,
                Body = "Post 4 Body",
                CreatedAt = _seedTime,
            },
            new Post
            {
                Id = 5,
                AuthorId = 3,
                Body = "Post 5 Body",
                CreatedAt = _seedTime,
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
                Body = "Comment 1 Body",
                CreatedAt = _seedTime,
            },
            new Comment
            {
                Id = 2,
                PostId = 2,
                UserId = 2,
                Body = "Comment 2 Body",
                CreatedAt = _seedTime,
            },
            new Comment
            {
                Id = 3,
                PostId = 2,
                UserId = 3,
                Body = "Comment 3 Body",
                CreatedAt = _seedTime,
            },
            new Comment
            {
                Id = 4,
                PostId = 2,
                UserId = 1,
                Body = "Comment 4 Body",
                CreatedAt = _seedTime,
            },
            new Comment
            {
                Id = 5,
                PostId = 3,
                UserId = 1,
                Body = "Comment 5 Body",
                CreatedAt = _seedTime,
            }
        );
    }

    private static void SeedLikes(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Like>().HasData(
            new Like
            {
                Id = 1,
                PostId = 1,
                UserId = 1
            },
            new Like
            {
                Id = 2,
                PostId = 1,
                UserId = 2
            },
            new Like
            {
                Id = 3,
                PostId = 1,
                UserId = 3
            }
            );
    }

    private static void SeedCourses(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                Id = 1,
                Name = "Course 1",
            },
            new Course
            {
                Id = 2,
                Name = "Course 2",
            },
            new Course
            {
                Id = 3,
                Name = "Course 3",
            },
            new Course
            {
                Id = 4,
                Name = "Course 4",
            },
            new Course
            {
                Id = 5,
                Name = "Course 5",
            }
        );
    }

    private static void SeedCohorts(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cohort>().HasData(
            new Cohort
            {
                Id = 1,
                CohortNumber = 1,
                CohortName = "August 2025",
                StartDate = new DateTime(2025, 8, 1),
                EndDate = new DateTime(2025, 9, 29),
            },
            new Cohort
            {
                Id = 2,
                CohortNumber = 2,
                CohortName = "February 2026",
                StartDate = new DateTime(2026, 2, 1),
                EndDate = new DateTime(2026, 3, 29),
            }


        );
    }

    private static void SeedCohortCourses(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CohortCourse>().HasData(
            new CohortCourse
            {
                Id = 1,
                CohortId = 1,
                CourseId = 1,                
            },
            new CohortCourse
            {
                Id = 2,
                CohortId = 1,
                CourseId = 2
            },
            new CohortCourse
            {
                Id = 3,
                CohortId = 2,
                CourseId = 1
            }


        );
    }

    private static void SeedUserCC(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCC>().HasData(
            new UserCC
            {
                Id = 1,
                CcId = 1,
                UserId = 1
            },
            new UserCC
            {
                Id = 2,
                CcId = 1,
                UserId = 2,
            },
            new UserCC
            {
                Id = 3,
                CcId = 1,
                UserId = 3
            }


        );
    }

    private static void SeedUserExercises(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCC>().HasData(
            new UserExercise
            {
                Id = 1,
                SubmissionLink = "subLink 1",
                SubmitionTime = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                Grade = 0,
                UserId = 1,
                Submitted = true,
                ExerciseId = 1
            },
            new UserExercise
            {
                Id = 2,
                SubmissionLink = "subLink 2",
                SubmitionTime = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                Grade = 3,
                UserId = 2,
                Submitted = true,
                ExerciseId = 1
            },
            new UserExercise
            {
                Id = 3,
                SubmissionLink = "subLink 3",
                SubmitionTime = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
                Grade = 0,
                UserId = 3,
                Submitted = false,
                ExerciseId = 1
            }


        );
    }



    private static void SeedModules(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Module>().HasData(
            new Module
            {
                Id = 1,
                Title = "Module 1"
            },
            new Module
            {
                Id = 2,
                Title = "Module 2"
            },
            new Module
            {
                Id = 3,
                Title = "Module 3"
            }
        );
    }

    private static void SeedCourseModules(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseModule>().HasData(
            new CourseModule
            {
                Id = 1,
                CourseId = 1,
                ModuleId = 1
            },
            new CourseModule
            {
                Id = 2,
                CourseId = 2,
                ModuleId = 2
            },
            new CourseModule
            {
                Id = 3,
                CourseId = 2,
                ModuleId = 1
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
                Name = "Unit 1",
            },
            new Unit
            {
                Id = 2,
                ModuleId = 1,
                Name = "Unit 2",
            },
            new Unit
            {
                Id = 3,
                ModuleId = 2,
                Name = "Unit 3",
            },
            new Unit
            {
                Id = 4,
                ModuleId = 2,
                Name = "Unit 4",
            },
            new Unit
            {
                Id = 5,
                ModuleId = 2,
                Name = "Unit 5",
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
                Name = "Exercise 1",
                GitHubLink = "",
                Description = "Exercise 1 description"
            },
            new Exercise
            {
                Id = 2,
                UnitId = 2,
                Name = "Exercise 2",
                GitHubLink = "",
                Description = "Exercise 2 description"
            },
            new Exercise
            {
                Id = 3,
                UnitId = 3,
                Name = "Exercise 3",
                GitHubLink = "",
                Description = "Exercise 3 description"
            },
            new Exercise
            {
                Id = 4,
                UnitId = 3,
                GitHubLink = "",
                Name = "Exercise 4",
                Description = "Exercise 4 description"
            },
            new Exercise
            {
                Id = 5,
                UnitId = 4,
                GitHubLink = "",
                Name = "Exercise 5",
                Description = "Exercise 5 description"
            }
        );
    }
    private static void SeedNotes(ref ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<Note>().HasData(
            new Note
            {
                Id = 1,
                UserId = 1,
                Title = "Name Note 1",
                Content = "note1note1 note1 note1 content",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc)
            },
            new Note
            {
                Id = 2,
                UserId = 2,
                Title = "Name Note 2",
                Content = "note2 note2 note2 note2 content",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc)
            },
            new Note
            {
                Id = 3,
                UserId = 1,
                Title = "Name Note 3",
                Content = "note3 note3 note3 note3 content",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc)
            },
            new Note
            {
                Id = 4,
                UserId = 1,
                Title = "Name Note 4",
                Content = "note4 note4 note4 note4 content",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc)
            },
            new Note
            {
                Id = 5,
                UserId = 1,
                Title = "Name Note 5",
                Content = "note5 note5 note5 note5 content",
                CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc)
            }
        );
    }
}