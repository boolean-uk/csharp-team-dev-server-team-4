using exercise.wwwapi.Models.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public static class ModelSeeder
    {
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
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test1test1%"),
                },
                new Credential
                {
                    Email = "test2@test2",
                    UserId = 2,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test2test2%"),
                },
                new Credential
                {
                    Email = "test3@test3",
                    UserId = 3,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test3test3%"),
                },
                new Credential
                {
                    Email = "test4@test4",
                    UserId = 4,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test4test4%"),
                },
                new Credential
                {
                    Email = "test5@test5",
                    UserId = 5,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test5test5%"),
                }
            );
        }

        private static void SeedProfiles(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>().HasData(
                new Profile()
                {
                    UserId = 1,
                },
                new Profile()
                {
                    UserId = 2,
                },
                new Profile()
                {
                    UserId = 3,
                },
                new Profile()
                {
                    UserId = 4,
                },
                new Profile()
                {
                    UserId = 5,
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