using exercise.wwwapi.Enums;
using exercise.wwwapi.Models.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public static class ModelSeeder
    {
        // Necessary because the seeder requires them to be constant values 
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
            SeedCredentials(ref modelBuilder);
            SeedProfiles(ref modelBuilder);
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
                    LastName = "",
                    FirstName = "",
                    Github = "",
                    Bio = "",
                },
                new Profile()
                {
                    UserId = 2,
                    LastName = "",
                    FirstName = "",
                    Github = "",
                    Bio = "",
                },
                new Profile()
                {
                    UserId = 3,
                    LastName = "",
                    FirstName = "",
                    Github = "",
                    Bio = "",
                },
                new Profile()
                {
                    UserId = 4,
                    LastName = "",
                    FirstName = "",
                    Github = "",
                    Bio = "",
                },
                new Profile()
                {
                    UserId = 5,
                    LastName = "",
                    FirstName = "",
                    Github = "",
                    Bio = "",
                }
            );
        }

        private static void SeedUsers(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                },
                new User
                {
                    Id = 2,
                },
                new User
                {
                    Id = 3,
                },
                new User
                {
                    Id = 4,
                },
                new User
                {
                    Id = 5,
                }
            );
        }
    }
}