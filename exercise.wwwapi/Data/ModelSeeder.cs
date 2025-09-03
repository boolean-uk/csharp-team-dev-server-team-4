using exercise.wwwapi.Models.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Data
{
    public static class ModelSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Credential = new Credential
                    {
                        Email = "test1@test1",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test1test1%")
                    }
                },
                new User
                {
                    Id = 2,
                    Credential = new Credential
                    {
                        Email = "test2@test2",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test2test2%")
                    }
                },
                new User
                {
                    Id = 3,
                    Credential = new Credential
                    {
                        Email = "test3@test3",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test3test3%")
                    }
                },
                new User
                {
                    Id = 4,
                    Credential = new Credential
                    {
                        Email = "test4@test4",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test4test4%")
                    }
                },
                new User
                {
                    Id = 5,
                    Credential = new Credential
                    {
                        Email = "test5@test5",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test5test5%")
                    }
                }
            );
        }
    }
}